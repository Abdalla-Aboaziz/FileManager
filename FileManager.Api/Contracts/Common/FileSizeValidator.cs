namespace FileManager.Api.Contracts.Common
{
    public class FileSizeValidator:AbstractValidator<IFormFile>
    {
        public FileSizeValidator()
        {
            Size Validation
            RuleFor(x => x)  // x refers to file 
                .Must((request, context) => request.Length <= FileSettings.MaxFileSizeInBytes)
                .WithMessage($"Max File Size is {FileSettings.MaxFileSizeInMB} MB.")
                .When(x => x is not null);
        }
    }
}
