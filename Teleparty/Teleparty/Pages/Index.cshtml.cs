using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Teleparty.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://ophimmoi.net/category/chieu-rap/");
            request.Method = HttpMethod.Get;

            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "Thunder Client (https://www.thunderclient.com)");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
        }
    }
}