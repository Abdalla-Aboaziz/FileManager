namespace FileManager.Api.Contracts.Common
{
    public class BlockedSignaturesValidator:AbstractValidator<IFormFile>
    {
        public BlockedSignaturesValidator()
        {
            // Signeture
            RuleFor(x => x)
                .Must((request, context) =>
                {
                    BinaryReader binary = new(request.OpenReadStream());
                    var bytes = binary.ReadBytes(2);
                    var fileSequenceHex = BitConverter.ToString(bytes);

                    foreach (var signature in FileSettings.BlockedSignatures)
                        if (signature.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                            return false;

                    return true;
                })
                .WithMessage("Not Allowed File Content")
                .When(x => x is not null);
        }
    }
}
