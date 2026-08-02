namespace FileManager.Api.Contracts.Common
{
    public class FileNameValidator:AbstractValidator<IFormFile>
    {
        public FileNameValidator()
        {
            //RuleFor(x => x.FileName)
            //    .Matches(RegexPatterns.FileNamePattern)
            //    .WithMessage(" InValid File Name ")
            //    .When(x => x is not null);
        }
    }
}
