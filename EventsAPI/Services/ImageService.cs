namespace EventsAPI.Services
{
    public class ImageService
    {
        private readonly IWebHostEnvironment _environment;
        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            if (!IsImage(image))
            {
                throw new ArgumentException("Переданный файл не является поддерживаемой картинкой");
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return $"/images/{uniqueFileName}";
        }

        private static bool IsImage(IFormFile file)
        {
            var validImageTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            return validImageTypes.Contains(file.ContentType);
        }
    }
}
