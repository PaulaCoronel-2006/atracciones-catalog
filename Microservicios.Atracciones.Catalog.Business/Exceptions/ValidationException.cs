namespace Microservicios.Atracciones.Catalog.Business.Exceptions;

/// <summary>
/// ExcepciÃ³n lanzada cuando los datos de entrada no cumplen con las reglas de validaciÃ³n.
/// Contiene la lista de errores descriptivos para devolver al cliente.
/// </summary>
[System.Diagnostics.DebuggerNonUserCode]
public class ValidationException : Exception
{
    public List<string> Errors { get; }

    public ValidationException(List<string> errors)
        : base("Uno o mÃ¡s errores de validaciÃ³n ocurrieron.")
    {
        Errors = errors;
    }

    public ValidationException(string error)
        : base(error)
    {
        Errors = [error];
    }
}

