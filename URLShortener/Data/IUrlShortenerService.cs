namespace URLShortener.Data
{
    public interface IUrlShortenerService
    {
        Task<UrlShortener> ShortenUrl(UrlShortenerRequest request);
        Task<UrlShortener> GetLongUrl(string shortUrl);

        string GenerateShortUrl(string url);
    }
}
