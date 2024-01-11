using Microsoft.EntityFrameworkCore;
using URLShortener.Data;

namespace URLShortener
{
    public class UrlShortenerDbContext : DbContext
    {
        //public DbSet<UrlShortenerRequest> UrlShortenerRequests { get; set; }
        public DbSet<UrlShortener> UrlShorteners { get; set; }

        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options)
        {
        }
    }
}
