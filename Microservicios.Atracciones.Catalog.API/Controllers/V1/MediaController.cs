using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microservicios.Atracciones.Catalog.Business.Interfaces;

namespace Microservicios.Atracciones.Catalog.API.Controllers.V1;

[ApiController]
[Route("api/v1/media")]
[Authorize(Roles = "Admin,Partner")] // Solo admins y partners pueden subir fotos
public class MediaController : ControllerBase
{
    private readonly IStorageService _storageService;

    public MediaController(IStorageService storageService)
    {
        _storageService = storageService;
    }

    /// <summary>
    /// Sube un archivo de imagen (form-data) y devuelve la URL estÃ¡tica/pÃºblica generada.
    /// Esta URL luego se inyecta en el JSON de CreateCompleteAttractionRequest.
    /// </summary>
    /// <param name="file">Archivo de imagen (ej. jpg, png)</param>
    /// <returns>Objeto JSON con la propiedad 'url'</returns>
    [HttpPost("upload")]
    public async Task<ActionResult> UploadImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { message = "No se enviÃ³ ningÃºn archivo o el archivo estÃ¡ vacÃ­o." });
        }

        try
        {
            // Opcional: Validar extensiones permitidas (jpeg, png, webp, etc.)
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest(new { message = "Solo se permiten archivos de imagen (.jpg, .png, .webp)." });
            }

            // Opcional: Validar tamaÃ±o mÃ¡ximo (ej. 5MB)
            if (file.Length > 5 * 1024 * 1024)
            {
                return BadRequest(new { message = "El archivo excede el tamaÃ±o mÃ¡ximo permitido de 5MB." });
            }

            // Delegamos el guardado fÃ­sico a nuestro StorageService genÃ©rico.
            // Hoy guarda en local (/wwwroot/uploads), maÃ±ana en Azure Blob.
            var fileUrl = await _storageService.SaveFileAsync(file, "attractions");

            return Ok(new { url = fileUrl });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error interno al guardar la imagen.", details = ex.Message });
        }
    }
}

