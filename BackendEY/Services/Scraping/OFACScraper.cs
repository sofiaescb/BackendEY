using BackendEY.DTOs;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Text;


namespace BackendEY.Services.Scraping
{
    public class OFACScraper : IScraperStrategy
    {
        private readonly HttpClient _client;

        public string Fuente => "ofac";

        public OFACScraper()
        {
            _client = new HttpClient(new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            });
            _client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        }

        public async Task<ScrapingResultDTO> BuscarAsync(string term)
        {
            try
            {
                var getResponse = await _client.GetAsync("https://sanctionssearch.ofac.treas.gov/");
                var getContent = await getResponse.Content.ReadAsStringAsync();

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(getContent);

                string? viewState = htmlDoc.GetElementbyId("__VIEWSTATE")?.GetAttributeValue("value", "");
                string? viewStateGen = htmlDoc.GetElementbyId("__VIEWSTATEGENERATOR")?.GetAttributeValue("value", "");

                if (string.IsNullOrEmpty(viewState) || string.IsNullOrEmpty(viewStateGen))
                    return new ScrapingResultDTO { 
                        Fuente = this.Fuente,
                        TotalResultados = 0,
                        Resultados = [],
                        Error = "No se pudo obtener el VIEWSTATE" };

                
                var postData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("__VIEWSTATE", viewState),
                    new KeyValuePair<string, string>("__VIEWSTATEGENERATOR", viewStateGen),
                    new KeyValuePair<string, string>("__EVENTTARGET", ""),
                    new KeyValuePair<string, string>("__EVENTARGUMENT", ""),
                    new KeyValuePair<string, string>("ctl00$MainContent$txtLastName", term),
                    new KeyValuePair<string, string>("ctl00$MainContent$Slider1", "100"),
                    new KeyValuePair<string, string>("ctl00$MainContent$Slider1_Boundcontrol", "100"),
                    new KeyValuePair<string, string>("ctl00$MainContent$btnSearch", "Search")
                });

                
                var postResponse = await _client.PostAsync("https://sanctionssearch.ofac.treas.gov/", postData);
                var postContent = await postResponse.Content.ReadAsStringAsync();

                var postDoc = new HtmlDocument();
                postDoc.LoadHtml(postContent);

                var table = postDoc.GetElementbyId("gvSearchResults");
                if (table == null)
                    return new ScrapingResultDTO {
                        Fuente = this.Fuente, 
                        TotalResultados = 0, 
                        Resultados = [],
                        Error = "No se encontró la tabla de resultados." };

                var rows = table.SelectNodes(".//tr");
                var results = new List<Dictionary<string, string>>();

                if(rows == null)
                    return new ScrapingResultDTO {
                        Fuente = this.Fuente, 
                        TotalResultados = 0, 
                        Resultados = [],
                        Error = "No se encontraron resultados." };

                foreach (var row in rows)
                {
                    var cols = row.SelectNodes("td");
                    if (cols != null && cols.Count >= 6)
                    {
                        results.Add(new Dictionary<string, string>
                        {
                            { "Name", cols[0].InnerText.Trim() },
                            { "Address", cols[1].InnerText.Trim() },
                            { "Type", cols[2].InnerText.Trim() },
                            { "Programs", cols[3].InnerText.Trim() },
                            { "List", cols[4].InnerText.Trim() },
                            { "Score", cols[5].InnerText.Trim() }
                        });
                    }
                }

                return new ScrapingResultDTO {
                    Fuente = this.Fuente,
                    TotalResultados = results.Count,
                    Resultados = results};
            }
            catch (Exception ex)
            {
                return new ScrapingResultDTO {
                    Fuente = this.Fuente,
                    TotalResultados = 0,
                    Resultados = [],
                    Error = ex.Message };
            }
        }
    }
}
