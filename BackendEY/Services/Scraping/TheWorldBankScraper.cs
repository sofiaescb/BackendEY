using BackendEY.DTOs;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendEY.Services.Scraping
{
    public class TheWorldBankScraper : IScraperStrategy
    {
        public string Fuente => "worldbank";

        public async Task<ScrapingResultDTO> BuscarAsync(string nombre)
        {
            try
            {
                var resultado = await Task.Run(() =>
                {
                    var resultados = new List<Dictionary<string, string>>();

                    var options = new ChromeOptions();
                    options.AddArgument("--headless");
                    options.AddArgument("--disable-gpu");
                    options.AddArgument("--no-sandbox");

                    using var driver = new ChromeDriver(options);
                    driver.Navigate().GoToUrl("https://projects.worldbank.org/en/projects-operations/procurement/debarred-firms");

                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                    wait.Until(d =>
                        d.FindElements(By.CssSelector("div.k-grid-content tr[role='row']")).Count > 0);

                    var rows = driver.FindElements(By.CssSelector("div.k-grid-content tr[role='row']"));

                    foreach (var row in rows)
                    {
                        var cells = row.FindElements(By.TagName("td"));
                        if (cells.Count < 7) continue;

                        var firmName = cells[0].Text.Trim();
                        if (!firmName.ToLower().Contains(nombre.ToLower())) continue;

                        resultados.Add(new Dictionary<string, string>
                        {
                            ["FirmName"] = firmName,
                            ["Address"] = cells[2].Text.Trim(),
                            ["Country"] = cells[3].Text.Trim(),
                            ["FromDate"] = FormatearFecha(cells[4].Text.Trim()),
                            ["ToDate"] = FormatearFecha(cells[5].Text.Trim()),
                            ["Grounds"] = cells[6].Text.Trim()
                        });
                    }

                    driver.Quit();

                    return new ScrapingResultDTO
                    {
                        Fuente = Fuente,
                        TotalResultados = resultados.Count,
                        Resultados = resultados,
                        Error = resultados.Count == 0 ? "No se encontraron coincidencias." : null
                    };
                });

                return resultado;
            }
            catch (Exception ex)
            {
                return new ScrapingResultDTO
                {
                    Fuente = Fuente,
                    TotalResultados = 0,
                    Resultados = new(),
                    Error = $"Error en scraping de World Bank: {ex.Message}"
                };
            }
        }

        private string FormatearFecha(string texto)
        {
            string[] formatos = { "dd-MMM-yyyy", "d-MMM-yyyy", "dd-MMM-yy", "d-MMM-yy" };
            if (DateTime.TryParseExact(texto, formatos, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime fecha))
            {
                return fecha.ToString("yyyy-MM-dd");
            }

            return texto;
        }
    }
}
