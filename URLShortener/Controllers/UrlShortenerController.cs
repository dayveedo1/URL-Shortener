using Microsoft.AspNetCore.Mvc;
using URLShortener.Data;
using URLShortener.Data.Validation;

namespace URLShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : Controller
    {
       
        private readonly IUrlShortenerService service;

        public UrlShortenerController(IUrlShortenerService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> PostUrl(UrlShortenerRequest request)
        {
            UrlShortenerRequestValidator validator = new();
            var validatorResult = validator.Validate(request);

            if (!validatorResult.IsValid)
                return BadRequest(validatorResult.Errors);

            var result = await service.ShortenUrl(request);
            return Ok(result);
        }

        [HttpGet("{shortUrl}")]
        public async Task<ActionResult<UrlShortener>> GetLongUrl(string shortUrl)
        {
            var result = await service.GetLongUrl(shortUrl);

            if (result == null)
                return NotFound();

            return result;
        }
    }
}
