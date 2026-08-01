
namespace FileManager.Api.Contracts
{
    public class UploadFileRequestValidator:AbstractValidator<UploadFileRequest>
    {
        public UploadFileRequestValidator()
        {
            RuleFor(x => x.File)
                .SetValidator(new FileSizeValidator());

            // Signeture
            RuleFor(x => x.File)
                .SetValidator(new BlockedSignaturesValidator());

            RuleFor(x => x.File)
                .SetValidator(new FileNameValidator());
        }
    }
}
