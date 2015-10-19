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
        public static Node Create(string path)
        {
            var node = new Node();

            if (IsDirectory(path))
            {
                node.Name = GetDirectoryName(path);
                node.Path = GetDirectoryPath(path);
                node.ParentPath = GetParentDirectoryPath(path);
                node.Type = Node.DirectoryTypeName;
            }
            else
            {
                node.Name = Path.GetFileName(path);
                node.Path = path;
                node.ParentPath = GetFileDirectoryPath(path);
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
                ParentPath = null,
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

        public string ParentPath { get; set; }
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

        public static List<Node> GetChildren(string path)
        {
            var node = NodeFactory.Create(path);

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
                            Node newDirectoryNode = NodeFactory.Create(directoryInfo.FullName);
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
                        Node newFileNode = NodeFactory.Create(fileInfo.FullName);
                        childrenNodes.Add(newFileNode);
                    }
                }
            }

            return childrenNodes;
        }

    }
}