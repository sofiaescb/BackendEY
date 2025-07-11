using BackendEY.DTOs;
using BackendEY.Services.Scraping;

namespace BackendEY.Services
{
    public class WebScrapingService
    {
        private readonly IEnumerable<IScraperStrategy> _scrapers;

        public WebScrapingService(IEnumerable<IScraperStrategy> scrapers)
        {
            _scrapers = scrapers;
        }

        public Task<ScrapingResultDTO> EjecutarBusqueda(ScrapingRequestDTO request)
        {
            var strategy = _scrapers.FirstOrDefault(s => s.Fuente.Equals(request.Fuente, StringComparison.OrdinalIgnoreCase));

            if (strategy == null)
                throw new Exception($"Fuente no soportada: {request.Fuente}");

            return strategy.BuscarAsync(request.Proveedor);
        }
    }
}
