using EventsAPI.BLL.Interfaces;

namespace EventsAPI.Adapters
{
    public class FormFileAdapter : IImageFile
    {
        private readonly IFormFile? _formFile;
        public FormFileAdapter(IFormFile? formFile)
        {
            _formFile = formFile;
        }
        
        public string FileName => _formFile.FileName;
        public string ContentType => _formFile.ContentType;
        public long Length => _formFile.Length;
        public async Task CopyToAsync(Stream target)
        {
            await _formFile.CopyToAsync(target);
        }
    }
}
