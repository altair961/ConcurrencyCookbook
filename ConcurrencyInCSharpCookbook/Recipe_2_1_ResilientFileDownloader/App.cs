using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_2_1_ResilientFileDownloader
{
    internal class App
    {
        private readonly IDownload _downloader;
        public App(IDownload downloader)
        {
            _downloader = downloader;
        }
        public async Task Run() 
        {
            var r = await _downloader.Download();
        }
    }
}
