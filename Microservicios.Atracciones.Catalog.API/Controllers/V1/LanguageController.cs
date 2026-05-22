using Microsoft.AspNetCore.Mvc;
using Microservicios.Atracciones.Catalog.Business.DTOs.Master;
using Microservicios.Atracciones.Catalog.Business.Interfaces;

namespace Microservicios.Atracciones.Catalog.API.Controllers.V1;

[ApiController]
[Route("api/v1/language")]
public class LanguageController : ControllerBase
{
    private readonly IMasterDataService _masterData;

    public LanguageController(IMasterDataService masterData)
    {
        _masterData = masterData;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LanguageResponse>>> GetAll()
    {
        var result = await _masterData.GetLanguagesAsync();
        return Ok(result);
    }
}

