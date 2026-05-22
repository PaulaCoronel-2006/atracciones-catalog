using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microservicios.Atracciones.Catalog.Business.DTOs.Common;

namespace Microservicios.Atracciones.Catalog.API.Filters;

/// <summary>
/// Intercepta las respuestas exitosas de los controladores y las envuelve automÃ¡ticamente
/// en el formato estÃ¡ndar { success: true, data: ..., message: ... } para evitar cÃ³digo repetitivo.
/// </summary>
public class ApiResponseWrapperFilter : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is not ObjectResult objectResult)
        {
            await next();
            return;
        }

        var isAlreadyWrapped = objectResult.Value != null && 
                               objectResult.Value.GetType().IsGenericType &&
                               (objectResult.Value.GetType().GetGenericTypeDefinition() == typeof(ApiResponse<>) ||
                                objectResult.Value.GetType().GetGenericTypeDefinition() == typeof(PagedApiResponse<>));

        if (isAlreadyWrapped)
        {
            await next();
            return;
        }

        if (objectResult.StatusCode >= 200 && objectResult.StatusCode < 300)
        {
            var valueType = objectResult.Value?.GetType() ?? typeof(object);
            var genericResponseType = typeof(ApiResponse<>).MakeGenericType(valueType);
            
            var wrappedResponse = Activator.CreateInstance(genericResponseType);
            
            var successProp = genericResponseType.GetProperty("Success");
            var dataProp = genericResponseType.GetProperty("Data");
            
            successProp?.SetValue(wrappedResponse, true);
            dataProp?.SetValue(wrappedResponse, objectResult.Value);

            objectResult.Value = wrappedResponse;
        }

        await next();
    }
}

