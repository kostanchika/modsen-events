using EventsAPI.BLL.Interfaces;

namespace EventsAPI.Adapters
{
    public class WebHostAdapter : IImagePath
    {
        private readonly IWebHostEnvironment _environment;
        public WebHostAdapter(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public string RootPath => _environment.WebRootPath;
    }
}
