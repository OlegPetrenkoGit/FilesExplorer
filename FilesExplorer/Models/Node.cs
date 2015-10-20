using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FilesExplorer.Models
{
    public enum NodeType
    {
        Drive,
        Directory,
        File
    }

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

        private static string GetDirectoryPath(string path)
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

    public class DirectoryNodes
    {
        public static List<Node> GetDrives()
        {
            var driveNodes = new List<Node>();

            var driveInfos = DriveInfo.GetDrives();
            foreach (var driveInfo in driveInfos)
            {
                var driveNode = NodeFactory.CreateDrive(driveInfo);
                driveNodes.Add(driveNode);
            }

            return driveNodes;
        }

        public static List<Node> GetChildren(Node node)
        {
            var childrenNodes = new List<Node>();

            if (NodeFactory.IsDirectory(node.Path))
            {
                DirectoryInfo rootDirectoryInfo = new DirectoryInfo(node.Path);
                //   try
                {
                    DirectoryInfo[] subDirectoryInfos = rootDirectoryInfo.GetDirectories();

                    foreach (DirectoryInfo directoryInfo in subDirectoryInfos)
                    {
                        if (!Node.IsHidden(directoryInfo))
                        {
                            Node newDirectoryNode = NodeFactory.Create(directoryInfo.FullName, parent: node);
                            childrenNodes.Add(newDirectoryNode);
                        }
                    }
                }
                //  catch (UnauthorizedAccessException) { }
            }

            if (Node.HasFiles(node))
            {
                var parentDirectoryInfo = new DirectoryInfo(node.Path);

                var rootDirectoryFileInfos = parentDirectoryInfo.EnumerateFiles();
                foreach (FileInfo fileInfo in rootDirectoryFileInfos)
                {
                    if (!Node.IsHidden(fileInfo))
                    {
                        Node newFileNode = NodeFactory.Create(fileInfo.FullName, parent: node);
                        childrenNodes.Add(newFileNode);
                    }
                }
            }

            return childrenNodes;
        }
    }
}