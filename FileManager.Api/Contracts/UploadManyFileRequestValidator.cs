namespace FileManager.Api.Contracts
{
    public class UploadManyFileRequestValidator:AbstractValidator<UploadManyFilesRequest>
    {
        public UploadManyFileRequestValidator()
        {
            RuleForEach(x => x.Files)
                .SetValidator(new FileSizeValidator());

            RuleForEach(x => x.Files)
                .SetValidator(new BlockedSignaturesValidator());

            RuleForEach(x => x.Files)
                .SetValidator(new FileNameValidator());
        }
    }
}
