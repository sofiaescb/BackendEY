using Azure.Core;
using BackendEY.DTOs;
using BackendEY.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendEY.Controllers
{
    [Route("api/screening")]
    [ApiController]
    [Authorize]
    public class WebScrapingController : ControllerBase
    {
        private readonly WebScrapingService _scrapingService;

        public WebScrapingController(WebScrapingService scrapingService)
        {
            _scrapingService = scrapingService;
        }

        [HttpGet]
        public async Task<ActionResult<ScrapingResultDTO>> Get([FromQuery] ScrapingRequestDTO request)
        {
            try
            {
                var resultado = await _scrapingService.EjecutarBusqueda(request);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
