using System;
using FluentValidation;

namespace KochWermann.SKS.Package.BusinessLogic.Validators
{
    public class UrlValidator : AbstractValidator<string>
    {
        public UrlValidator()
        {
            RuleFor(c => c)
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uriResult) 
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps));
        }
    }
}