using FilesExplorer.Models;
using System;
using System.IO;

public class FileCounters
{
    private const long tenmb = 10485760;
    private const long fiftymb = 52428800;

    public int Less { get; set; }
    public int Between { get; set; }
    public int More { get; set; }

    public FileCounters(Node node)
    {
        CountFiles(node.Path);
    }

    private void CountFiles(string path)
    {
        if (NodeFactory.IsDirectory(path))
        {
            try
            {
                var directoryInfo = new DirectoryInfo(path);

                var directories = directoryInfo.GetDirectories();
                if (directories.Length != 0)
                {
                    foreach (var directory in directories)
                    {
                        CountFiles(directory.FullName);
                    }
                }
                else
                {
                    CountFilesInDirectory(directoryInfo);
                }
            }
            catch (UnauthorizedAccessException) { }
        }
    }

    private void CountFilesInDirectory(DirectoryInfo directoryInfo)
    {
        var files = directoryInfo.GetFiles();
        foreach (var file in files)
        {
            var size = file.Length;
            if (size <= tenmb)
            {
                Less++;
            }
            else if (tenmb < size && size <= fiftymb)
            {
                Between++;
            }
            else
            {
                More++;
            }
        }
    }
}