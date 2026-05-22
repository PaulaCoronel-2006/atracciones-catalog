using System;

namespace Microservicios.Atracciones.Catalog.Business.Exceptions;

/// <summary>
/// ExcepciÃ³n que se lanza cuando hay un conflicto con el estado actual del servidor (ej: recurso duplicado).
/// Se mapea a HTTP 409 Conflict.
/// </summary>
[System.Diagnostics.DebuggerNonUserCode]
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}

