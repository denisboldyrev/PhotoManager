using System.IO;

namespace PhotoManager.Core.Interfaces.Services
{
    public interface IFileService
    {
        public void Delete(string path);
        public void Save(Stream stream, string path);
    }
}
