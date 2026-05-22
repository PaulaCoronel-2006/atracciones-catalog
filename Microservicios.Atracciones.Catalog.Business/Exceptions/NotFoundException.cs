
namespace Microservicios.Atracciones.Catalog.Business.Exceptions
{
    [System.Diagnostics.DebuggerNonUserCode]
    public class NotFoundException : BusinessException
    {
        public NotFoundException(string entity, object key)
            : base($"No se encontrÃ³ {entity} con la llave: {key}") { }
    }
}

