namespace VisualStudio.IO
{
    public interface IFileSystem
    {
        Maybe<string> FindFirst(string path, string filename);
    }
}
