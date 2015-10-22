using FilesExplorer.Models;
using System.IO;

public class NodeFactory
{
    public static Node Create(string path, Node parent)
    {
        var node = new Node();

        if (IsDirectory(path))
        {
            node.Name = GetDirectoryName(path);
            node.Path = GetDirectoryPath(path);
            node.Parent = parent;
            node.Type = Node.DirectoryTypeName;
        }
        else
        {
            node = new FileNode();
            node.Parent = parent;
            node.Name = Path.GetFileName(path);
            node.Path = path;
            node.Type = Node.FileTypeName;
        }

        return node;
    }

    public static Node CreateDrive(DriveInfo driveInfo)
    {
        var driveNode = new Node()
        {
            Name = driveInfo.Name,
            Path = driveInfo.RootDirectory.FullName,
            Parent = null,
            Type = Node.DriveTypeName
        };

        return driveNode;
    }

    public static bool IsDirectory(string path)
    {
        FileAttributes fileAttributes = File.GetAttributes(path);
        return fileAttributes.HasFlag(FileAttributes.Directory);
    }

    private static string GetDirectoryName(string path)
    {
        return new DirectoryInfo(path).Name;
    }

    public static string GetDirectoryPath(string path)
    {
        return new DirectoryInfo(path).FullName;
    }

    private static string GetFileDirectoryPath(string path)
    {
        return Path.GetDirectoryName(path);
    }

    private static string GetParentDirectoryPath(string path)
    {
        var directoryInfo = new DirectoryInfo(path);

        if (directoryInfo.Parent != null)
        {
            return directoryInfo.Parent.FullName;
        }
        else
        {
            return null;
        }
    }
}