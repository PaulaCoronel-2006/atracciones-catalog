using Microsoft.AspNetCore.Http;

namespace Microservicios.Atracciones.Catalog.Business.Interfaces;

/// <summary>
/// Interfaz para el guardado fÃ­sico de archivos.
/// Permite intercambiar fÃ¡cilmente entre almacenamiento local y en la nube (ej. Azure Blob Storage).
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Guarda un archivo fÃ­sico y devuelve su URL pÃºblica.
    /// </summary>
    /// <param name="file">El archivo subido por el cliente.</param>
    /// <param name="folder">Carpeta destino opcional (ej: "attractions", "avatars").</param>
    /// <returns>La URL pÃºblica absoluta donde se puede acceder al archivo.</returns>
    Task<string> SaveFileAsync(IFormFile file, string folder = "uploads");
}

