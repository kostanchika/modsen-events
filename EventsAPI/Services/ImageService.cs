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
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return $"/images/{uniqueFileName}";
        }
    }
}
