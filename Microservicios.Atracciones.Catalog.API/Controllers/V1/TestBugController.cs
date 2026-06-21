using Microsoft.AspNetCore.Mvc;
using Microservicios.Atracciones.Catalog.Business.Interfaces;
using Microservicios.Atracciones.Catalog.Business.DTOs.Attraction;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Microservicios.Atracciones.Catalog.API.Controllers.V1
{
    [ApiController]
    [Route("api/v1/testbug")]
    public class TestBugController : ControllerBase
    {
        private readonly IAttractionService _attractionService;

        public TestBugController(IAttractionService attractionService)
        {
            _attractionService = attractionService;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            try
            {
                var req = new CreateCompleteAttractionRequest
                {
                    Name = "Test Bug",
                    LocationId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    SubcategoryId = Guid.Parse("d4d4d4d4-d4d4-d4d4-d4d4-d4d4d4d4d4d4"),
                    DescriptionShort = "Test",
                    Tags = new List<Guid> { Guid.Parse("d1111111-1111-1111-1111-111111111111") },
                    Products = new List<ProductOptionRequest>
                    {
                        new ProductOptionRequest
                        {
                            Title = "Test",
                            PriceTiers = new List<PriceTierRequest> { new PriceTierRequest { TicketCategoryId = Guid.Parse("C3D4E5F6-A1B2-4C3D-0E1F-2A3B4C5D6E7F"), Price = 10, CurrencyCode = "USD" } }
                        }
                    }
                };
                var id = await _attractionService.CreateCompleteAsync(req, Guid.NewGuid(), true);
                return Ok(new { success = true, id });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, error = ex.ToString() });
            }
        }
    }
}
