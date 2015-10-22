using FilesExplorer.Models;
using System.IO;

public class FileNode : Node
{
    public long Size
    {
        get
        {
            var fileInfo = new FileInfo(Path);
            return fileInfo.Length;
        }
    }
}