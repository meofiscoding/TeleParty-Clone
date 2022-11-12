using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.RegularExpressions;

namespace Teleparty.Pages
{
    public class PartyModel : PageModel
    {
        [BindProperty]
        public string Source { get; set; }
        public async Task<IActionResult> OnGetAsync(string name)
        {
            name = RemoveSpecialCharacters(name);
            //Replace " " with "+" for name 
            name = name.Replace(" ", "+");
            //Search film
            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://ophimmoi.net/search/{name}/");
            request.Method = HttpMethod.Get;

            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "Thunder Client (https://www.thunderclient.com)");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();

            var web = new HtmlAgilityPack.HtmlDocument();
            web.LoadHtml(result);
            string innerText = "";
            var htmlnode = web.DocumentNode.SelectNodes("//div[@class='movie-title-1']");
            if (htmlnode == null)
            {
                ViewData["Err"] = "This film is currently not available";
                //Come back to Index page
                return RedirectToPage("/Index");
            }
            else
            {
                innerText = htmlnode[0].InnerText;
                innerText = RemoveSign4VietnameseString(innerText);
                innerText = innerText.ToLower().Replace(" ", "-");
            } 
            

            //Get Film Id
            var html = await GetFilmIdAsync(innerText);
            var pattern = @"filmInfo.filmID = parseInt\('([0-9]+)'\);";
            var id = Regex.Match(html, pattern).Groups[1].Value;

            //Get film src
            var srcFrame = await GetFilmSrc(Convert.ToInt32(id));
            var pattern2 =  "=(http.*)\"";
            Source = Regex.Match(srcFrame, pattern2).Groups[1].Value;
            return Page();
        }

        private async Task<string> GetFilmSrc(int id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://ophimmoi.net/wp-admin/admin-ajax.php");
            request.Method = HttpMethod.Post;

            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "Thunder Client (https://www.thunderclient.com)");

            var content = new MultipartFormDataContent();
            content.Add(new StringContent("halim_play_listsv"), "action");
            content.Add(new StringContent("1"), "episode");
            content.Add(new StringContent("1"), "server");
            content.Add(new StringContent($"{id}"), "postid");
            content.Add(new StringContent("1"), "ep_link");
            request.Content = content;

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private async Task<string> GetFilmIdAsync(string name)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://ophimmoi.net/xem-phim/{name}-tap-1-server-1/");
            request.Method = HttpMethod.Get;

            request.Headers.Add("Accept", "*/*");
            request.Headers.Add("User-Agent", "Thunder Client (https://www.thunderclient.com)");

            var response = await client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        private readonly string[] VietnameseSigns = new string[]   {

            "aAeEoOuUiIdDyY",

            "áàạảãâấầậẩẫăắằặẳẵ",

            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

            "éèẹẻẽêếềệểễ",

            "ÉÈẸẺẼÊẾỀỆỂỄ",

            "óòọỏõôốồộổỗơớờợởỡ",

            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

            "úùụủũưứừựửữ",

            "ÚÙỤỦŨƯỨỪỰỬỮ",

            "íìịỉĩ",

            "ÍÌỊỈĨ",

            "đ",

            "Đ",

            "ýỳỵỷỹ",

            "ÝỲỴỶỸ"
                };

        public string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public string RemoveSign4VietnameseString(string str)
        {
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

    }
}
