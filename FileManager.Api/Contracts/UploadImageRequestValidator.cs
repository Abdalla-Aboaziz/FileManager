namespace FileManager.Api.Contracts
{
    public class UploadImageRequestValidator:AbstractValidator<UploadImagesRequest>
    {
        public UploadImageRequestValidator()
        {
            RuleFor(x => x.Image)
                .SetValidator(new FileSizeValidator())
                .SetValidator(new FileNameValidator())
                .SetValidator(new BlockedSignaturesValidator());

            // check allow image extentions
            RuleFor(x => x.Image)
                .Must((request, context) =>
                {
                    var imageExtention = Path.GetExtension(request.Image.FileName.ToLower());
                    return FileSettings.AllowedImagesExtentions.Contains(imageExtention);
                })
                .WithMessage("File Extention is not allowed")
                .When(x => x.Image is not null);
        }
    }
}
