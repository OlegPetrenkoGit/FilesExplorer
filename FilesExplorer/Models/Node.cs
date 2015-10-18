using System.Collections.Generic;
using System.IO;

namespace FilesExplorer.Models
{
    public class Node
    {
        public virtual string Name { get; set; }
        public virtual string Path { get; set; }

        public Node(string name, string path)
        {
            Name = name;
            Path = path;
        }

        protected static bool IsHidden(FileSystemInfo fileSystemInfo)
        {
            return (fileSystemInfo.Attributes & FileAttributes.Hidden) != 0;
        }

        //public static List<Node> GetAllNodes()
        //{



        //    DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);
        //    DirectoryInfo[] subDirectoryInfos = rootDirectoryInfo.GetDirectories();
        //    foreach (DirectoryInfo directoryInfo in subDirectoryInfos)
        //    {
        //        if (!IsHidden(directoryInfo))
        //        {
        //            var newDirectoryNode = new DirectoryNode(directoryInfo);
        //            nodes.Add(newDirectoryNode);
        //        }
        //    }

        //    var rootDirectoryFileInfos = rootDirectoryInfo.EnumerateFiles();
        //    foreach (FileInfo fileInfo in rootDirectoryFileInfos)
        //    {
        //        if (!IsHidden(fileInfo))
        //        {
        //            var newFileNode = new FileNode(fileInfo);
        //            nodes.Add(newFileNode);
        //        }
        //    }

        //    return nodes;
        //}
    }

    public class DriveNode : Node
    {
        private DriveInfo driveInfo;

        public override string Name { get { return driveInfo.Name; } }
        public override string Path { get { return driveInfo.RootDirectory.FullName; } }
        public List<DirectoryNode> Directories { get; }

        public DriveNode(DriveInfo driveInfo) : base(driveInfo.Name, driveInfo.RootDirectory.FullName)
        {
            this.driveInfo = driveInfo;

            var directories = new List<DirectoryNode>();

            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(driveInfo.RootDirectory.FullName);
            DirectoryInfo[] subDirectoryInfos = rootDirectoryInfo.GetDirectories();
            foreach (DirectoryInfo directoryInfo in subDirectoryInfos)
            {
                if (!IsHidden(directoryInfo))
                {
                    var newDirectoryNode = new DirectoryNode(directoryInfo);
                    directories.Add(newDirectoryNode);
                }
            }

            Directories = directories;
        }
    }

    public class DirectoryNode : Node
    {
        private DirectoryInfo directoryInfo;

        public override string Name { get { return directoryInfo.Name; } }
        public override string Path { get { return directoryInfo.FullName; } }
        public List<FileNode> Files { get; }

        public DirectoryNode(DirectoryInfo directoryInfo) : base(directoryInfo.Name, directoryInfo.FullName)
        {
            this.directoryInfo = directoryInfo;

            var files = new List<FileNode>();

            var rootDirectoryFileInfos = directoryInfo.EnumerateFiles();
            foreach (FileInfo fileInfo in rootDirectoryFileInfos)
            {
                if (!IsHidden(fileInfo))
                {
                    var newFileNode = new FileNode(fileInfo);
                    files.Add(newFileNode);
                }
            }

            Files = files;
        }
    }

    public class FileNode : Node
    {
        private FileInfo fileInfo;

        public override string Name { get { return fileInfo.Name; } }
        public override string Path { get { return fileInfo.FullName; } }

        public long Size { get { return fileInfo.Length; } } //bytes

        public FileNode(FileInfo fileInfo) : base(fileInfo.Name, fileInfo.FullName)
        {
            this.fileInfo = fileInfo;
        }
    }

    public class TreeNode
    {
        public List<Node> Nodes { get; }

        public TreeNode()
        {
            Nodes = GetNodes();
        }

        private List<Node> GetNodes()
        {
            List<Node> nodes = new List<Node>();

            var drivesInfo = DriveInfo.GetDrives();

            foreach (DriveInfo driveInfo in drivesInfo)
            {
                var newDriveInfo = new DriveNode(driveInfo);
                nodes.Add(newDriveInfo);
            }

            return nodes;

            /*

            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);
            DirectoryInfo[] subDirectoryInfos = rootDirectoryInfo.GetDirectories();
            foreach (DirectoryInfo directoryInfo in subDirectoryInfos)
            {
                if (!IsHidden(directoryInfo))
                {
                    var newDirectoryNode = new DirectoryNode(directoryInfo);
                    nodes.Add(newDirectoryNode);
                }
            }

            var rootDirectoryFileInfos = rootDirectoryInfo.EnumerateFiles();
            foreach (FileInfo fileInfo in rootDirectoryFileInfos)
            {
                if (!IsHidden(fileInfo))
                {
                    var newFileNode = new FileNode(fileInfo);
                    nodes.Add(newFileNode);
                }
            }

           */
        }
    }
}