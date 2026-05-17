using Microsoft.Extensions.DependencyInjection;

namespace Recipe_2_1_ResilientFileDownloader
{
    internal class Program 
    {
        internal static async Task Main(string[] args) 
        {
            Console.WriteLine("heigh ho1");
            var services = new ServiceCollection();
            services.AddTransient<App>();
            services.AddHttpClient<IDownload, Downloader>(client =>
            {
                client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });
            var serviceProvider = services.BuildServiceProvider();
            var app = serviceProvider.GetRequiredService<App>();
 
            await app.Run();
        }
    }
}