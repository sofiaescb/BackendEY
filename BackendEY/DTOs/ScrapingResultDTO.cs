namespace BackendEY.DTOs
{
    public class ScrapingResultDTO
    {
        public required string Fuente { get; set; }              
        public required int TotalResultados { get; set; }
        public required List<Dictionary<string, string>> Resultados { get; set; }
        public string? Error { get; set; }
    }
}
