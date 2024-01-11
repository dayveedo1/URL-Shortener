using FluentValidation;

namespace URLShortener.Data.Validation
{
    public class UrlShortenerRequestValidator : AbstractValidator<UrlShortenerRequest>
    {
        public UrlShortenerRequestValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty()
                .Must(x => Uri.TryCreate(x, UriKind.Absolute, out var uri))
                .WithMessage("Invalid Url");
        }
    }
}
