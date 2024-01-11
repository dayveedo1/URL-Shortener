namespace URLShortener.Data
{
    public class UrlShortener
    {
        public int Id { get; set; }
        public string? LongUrl { get; set; }
        public string? ShortUrl { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
