using PhotoManager.Core.Interfaces.Services;
using System.IO;

namespace PhotoManager.BusinessLogic
{
    public class FileService : IFileService
    {
        public void Delete(string path)
        {
            File.Delete(path);
        }

        public void Save(Stream stream, string path)
        {
            using var fileStream = new FileStream(path, FileMode.Create);
            stream.CopyTo(fileStream);
        }
    }
}
