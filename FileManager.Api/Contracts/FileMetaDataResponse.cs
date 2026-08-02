namespace FileManager.Api.Contracts
{
    public record FileMetaDataResponse(
        
        Guid Id,
        string FileName,
        string ContentType,
        long Size,
        DateTime UploadedAt,
        string FileExtension

        );
 
}
