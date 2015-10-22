using System;
using System.IO;
using System.Linq;

namespace FilesExplorer.Models
{
    public class Node
    {
        public const string DriveTypeName = "drive";
        public const string DirectoryTypeName = "directory";
        public const string FileTypeName = "file";

        public Node Parent { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public static bool IsHidden(FileSystemInfo fileSystemInfo)
        {
            return (fileSystemInfo.Attributes & FileAttributes.Hidden) != 0;
        }

        public static bool IsEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        public static bool HasFiles(Node node)
        {
            Func<FileAttributes, bool> isFileNotHidden = (FileAttributes fileAttributes) => (fileAttributes & FileAttributes.Hidden) == 0;

            return (from file in Directory.EnumerateFiles(node.Path)
                    where isFileNotHidden(new FileInfo(file).Attributes)
                    select file).Any();
        }

        public override string ToString()
        {
            return Path;
        }
    }
}