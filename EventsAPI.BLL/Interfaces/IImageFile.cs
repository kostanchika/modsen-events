namespace EventsAPI.BLL.Interfaces
{
    public interface IImageFile
    {
        string FileName { get; }
        string ContentType { get; }
        long Length { get; }
        Task CopyToAsync(Stream target);
    }
}
