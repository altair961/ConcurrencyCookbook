namespace Recipe_2_1_ResilientFileDownloader
{
    public class Downloader : IDownload
    {
        private readonly HttpClient _http;

        public Downloader(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> Download()
        {
            Console.WriteLine("Download started...");
            // Sockets are pooled under the hood, preventing exhaustion
            var response = await _http.GetStringAsync("posts/1");
            return response;
        }
    }
}
