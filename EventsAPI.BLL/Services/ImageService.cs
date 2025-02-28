using EventsAPI.BLL.Interfaces;

namespace EventsAPI.BLL.Services
{
    public class ImageService
    {
        private readonly IImagePath _environment;
        public ImageService(IImagePath environment)
        {
            _environment = environment;
        }

        public async Task<string> UploadImageAsync(IImageFile image, CancellationToken ct = default)
        {
            if (!IsImage(image))
            {
                throw new ArgumentException("Переданный файл не является поддерживаемой картинкой");
            }

            var uploadsFolder = Path.Combine(_environment.RootPath, "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream, ct);
            }
            return $"/images/{uniqueFileName}";
        }

        private static bool IsImage(IImageFile file)
        {
            var validImageTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            return validImageTypes.Contains(file.ContentType);
        }
    }
}
