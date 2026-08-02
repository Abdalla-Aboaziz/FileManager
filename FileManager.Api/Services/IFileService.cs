



using FileManager.Api.Common;

namespace FileManager.Api.Services
{
    public interface IFileService
    {
        Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellationToken = default);
        Task<IEnumerable<Guid>> UploadManyAsync(IFormFileCollection files, CancellationToken cancellationToken = default);

        Task UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default);
        Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default);
        Task<(FileStream? stream, string contentType, string fileName)> StreamAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<FileMetaDataResponse?>  GetFileMetaDataAsync(Guid id, CancellationToken cancellationToken = default);
        Task<PaginatedList<FileMetaDataResponse>> GetUploadedFilesAsync(RequestFilters filters, CancellationToken cancellationToken = default);


    }
}
