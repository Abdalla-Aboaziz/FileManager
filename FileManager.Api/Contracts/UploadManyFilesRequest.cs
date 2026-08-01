namespace FileManager.Api.Contracts
{
    public record UploadManyFilesRequest
        ( 
                 IFormFileCollection Files   // or IEnumerable<IFormFile>
        );
   
}
