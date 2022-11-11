using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json;

namespace Teleparty.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Result
        {
            public bool adult { get; set; }
            public string backdrop_path { get; set; }
            public int id { get; set; }
            public string title { get; set; }
            public string original_language { get; set; }
            public string original_title { get; set; }
            public string overview { get; set; }
            public string poster_path { get; set; }
            public string media_type { get; set; }
            public List<int> genre_ids { get; set; }
            public double popularity { get; set; }
            public string release_date { get; set; }
            public bool video { get; set; }
            public double vote_average { get; set; }
            public int vote_count { get; set; }
        }

        public class Root
        {
            public int page { get; set; }
            public List<Result> results { get; set; }
            public int total_pages { get; set; }
            public int total_results { get; set; }
        }

        [BindProperty]
        public List<Result> Movie { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://api.themoviedb.org/3/trending/movie/week?api_key=5a626560913d4cbd29ba9b8ba06221ad&page=1");
            request.Method = HttpMethod.Get;

            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "Thunder Client (https://www.thunderclient.com)");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            var listmovies = JsonConvert.DeserializeObject<Root>(result);
            Movie = listmovies.results;
        }
    }
}