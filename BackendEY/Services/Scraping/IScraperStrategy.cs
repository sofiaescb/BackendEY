using BackendEY.DTOs;

namespace BackendEY.Services.Scraping
{
    public interface IScraperStrategy
    {
        string Fuente { get; }
        Task<ScrapingResultDTO> BuscarAsync(string term);
    }
}
