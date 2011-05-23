using System.IO;
using System.Linq;

namespace VisualStudio.IO
{
    public class FileSystem : IFileSystem
    {
        public Maybe<string> FindFirst(string path, string filename)
        {
            return Directory.EnumerateFileSystemEntries(path, filename, SearchOption.AllDirectories).FirstOrDefault().ToMaybe();
        }
    }
}
