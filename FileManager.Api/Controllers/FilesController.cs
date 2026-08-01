using FileManager.Api.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController(IFileService fileService): ControllerBase
    {
        private readonly IFileService _fileService = fileService;

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadFileRequest request ,CancellationToken cancellationToken)
        {
            var fileId = await _fileService.UploadAsync(request.File, cancellationToken);

            return CreatedAtAction(nameof(Download), new { id = fileId }, null);
        }
        [HttpPost("upload-many")]
        public async Task<IActionResult> UploadMany([FromForm] UploadManyFilesRequest request ,CancellationToken cancellationToken)
        {
            var filesIds = await _fileService.UploadManyAsync(request.Files, cancellationToken);

            return Ok(filesIds);
        }
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImagesRequest request ,CancellationToken cancellationToken)
        {
            await _fileService.UploadImageAsync(request.Image, cancellationToken);

            return Created();
        }
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download([FromRoute] Guid  id ,CancellationToken cancellationToken)
        {
            var (fileContent, contentType, fileName) = await _fileService.DownloadAsync(id, cancellationToken);

            return fileContent is [] ? NotFound() : File(fileContent, contentType, fileName);
        }

        [HttpGet("stream/{id}")]
        public async Task<IActionResult> Stream([FromRoute] Guid  id ,CancellationToken cancellationToken)
        {
            var (stream, contentType, fileName) = await _fileService.StreamAsync(id, cancellationToken);

            return stream is null ? NotFound() : File(stream, contentType, fileName,true);
        }

    }
}
