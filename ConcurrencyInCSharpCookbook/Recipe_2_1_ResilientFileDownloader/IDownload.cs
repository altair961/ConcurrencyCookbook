namespace Recipe_2_1_ResilientFileDownloader
{
    internal interface IDownload
    {
        Task<string> Download();
    }
}
