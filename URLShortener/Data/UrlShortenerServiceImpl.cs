using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Web;
using URLShortener.Data.Validation;

namespace URLShortener.Data
{
    public class UrlShortenerServiceImpl : IUrlShortenerService
    {

        public const int NumberOfCharactersInShortLink = 7;
        public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private readonly Random random = new Random();


        private UrlShortenerDbContext context;

        public UrlShortenerServiceImpl(UrlShortenerDbContext context)
        {
            this.context = context;
        }

        public async Task<UrlShortener> ShortenUrl (UrlShortenerRequest request)
        {
            if (String.IsNullOrEmpty(request.Url))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(request.Url));
            }

            //UrlShortenerRequestValidator validator = new();

            //var result = validator.Validate(request);

            //if (!result.IsValid)
            //    return new ArgumentException()



            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var uri))
                throw new ArgumentException("URL must be valid", nameof(request.Url));
            

            var shortUrl = GenerateShortUrl(request.Url);

            var encodedShortUrl = EncodeShortUrl(shortUrl);

            var urlShortener = new UrlShortener
            {
                LongUrl = request.Url,
                ShortUrl = encodedShortUrl  ,          //shortUrl,encodedShortUrl,
                DateCreated = DateTime.UtcNow
            };

            await context.AddAsync(urlShortener);
            await context.SaveChangesAsync();

            return new UrlShortener
            {
                LongUrl = urlShortener.LongUrl,
                ShortUrl = shortUrl,
                DateCreated = urlShortener.DateCreated
            };
        }

        
        public async Task<UrlShortener> GetLongUrl(string shortUrl)
        {
            if (String.IsNullOrEmpty(shortUrl))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(shortUrl));
            }


            var encodeShortUrl = EncodeShortUrl(shortUrl);

            var urlShortener = await context.UrlShorteners.Where(x => x.ShortUrl == encodeShortUrl).FirstOrDefaultAsync();
            if (urlShortener == null)
                throw new ArgumentException("URL does not exist", nameof(shortUrl));


            var decodedShortUrl = DecodeShortUrl(urlShortener.ShortUrl);

            return new UrlShortener
            {
                LongUrl = urlShortener.LongUrl,
                ShortUrl = decodedShortUrl,           
                DateCreated = urlShortener.DateCreated
            };

        }


        public string GenerateShortUrl(string url)
        {
            var codeChars = new char[NumberOfCharactersInShortLink];
            while (true)
            {
                for (var i = 0; i < NumberOfCharactersInShortLink; i++)
                {
                    var randomIndex = random.Next(Alphabet.Length);
                    codeChars[i] = Alphabet[randomIndex];
                }

                string? code = new string(codeChars);

                StringBuilder builder = new();

                builder.Append("https://ea.ly/" + code.ToLower());

                return builder.ToString();
            }

           


        }




        private static string EncodeShortUrl (string shortUrl)
        {
            string encodeUrl = HttpUtility.UrlEncode(shortUrl);

            return encodeUrl;
        }


        private static string DecodeShortUrl(string shortUrl)
        {
            string decodeUrl = HttpUtility.UrlDecode(shortUrl);

            return decodeUrl.ToString();

        }
    }
}
