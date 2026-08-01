namespace FileManager.Api.Services
{
    public class FileService(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context) : IFileService
    {
        // Physical path where uploaded files will be stored.
        // Example: wwwroot/uploads
        private readonly string _filePath = $"{webHostEnvironment.WebRootPath}/uploads";
        private readonly string _imagesPath = $"{webHostEnvironment.WebRootPath}/images";
        private readonly ApplicationDbContext _context = context;

        public async Task<Guid> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            var uploadedFile = await SaveFile(file, cancellationToken);
            

            // Save the file metadata in the database.
            await _context.AddAsync(uploadedFile, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            // Return the generated Id so the client can reference the uploaded file later.
            return uploadedFile.Id;

        }

      

        public async Task<IEnumerable<Guid>> UploadManyAsync(IFormFileCollection files, CancellationToken cancellationToken = default)
        {
            List<UploadedFiles> uploadedFiles = [];

            foreach (var  file in files)
            {
                var uploadedFile = await SaveFile(file, cancellationToken);
                uploadedFiles.Add(uploadedFile);

            }

            // Save the files metadata in the database.
            await _context.AddRangeAsync(uploadedFiles, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return uploadedFiles.Select(x => x.Id).ToList();

        }
        public async Task UploadImageAsync(IFormFile image, CancellationToken cancellationToken = default)
        {
           
            var path = Path.Combine(_imagesPath, image.FileName);

         
            using var stream = File.Create(path);
            await image.CopyToAsync(stream, cancellationToken);
        }

        public async Task<(byte[] fileContent, string contentType, string fileName)> DownloadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var file = await _context.Files.FindAsync(id);
            if (file is null)
                return ([],string.Empty ,string.Empty);
            var path = Path.Combine(_filePath, file.StoredFileName);

            MemoryStream memoryStream = new();
            using FileStream fileStream = new(path, FileMode.Open);
            fileStream.CopyTo(memoryStream);

            memoryStream.Position = 0;
            return (memoryStream.ToArray(), file.ContentType, file.FileName);
        }

        public async Task<(FileStream? stream, string contentType, string fileName)> StreamAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var file = await _context.Files.FindAsync(id);
            if (file is null)
                return (null, string.Empty, string.Empty);

            var path = Path.Combine(_filePath, file.StoredFileName);
            var fileStream = File.OpenRead(path);

            return (fileStream, file.ContentType, file.FileName);
        }
        private async Task<UploadedFiles> SaveFile (IFormFile file  , CancellationToken cancellationToken = default)
        {
            // Generate a random file name to prevent file name collisions
            // and avoid exposing the original file name on the server.

            var rondomFileName = Path.GetRandomFileName();

            // Create the database entity that stores the file metadata.
            // We keep the original file name for display purposes,
            // while the stored file name is the random one used on disk.
            var uploadedFile = new UploadedFiles
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                StoredFileName = rondomFileName,
                FileExtension = Path.GetExtension(file.FileName)
            };

            // Build the full physical path where the file will be saved.
            var path = Path.Combine(_filePath, rondomFileName);

            // Create the file on disk and copy the uploaded content into it.
            // "using" ensures the file stream is disposed automatically.
            using var stream = File.Create(path);
            await file.CopyToAsync(stream, cancellationToken);

            return uploadedFile;
        }

      
    }
}
