using FilesExplorer.Models;
using System;
using System.Collections.Generic;
using System.IO;

public class NodesCollection
{
    public List<Node> GetDrives()
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

    public List<Node> GetNodes(Node node)
    {
        var nodes = new List<Node>();

        if (NodeFactory.IsDirectory(node.Path))
        {
            var childrenDirectoryNodes = GetDirectoryNodes(node);
            nodes.AddRange(childrenDirectoryNodes);
        }

        if (Node.HasFiles(node))
        {
            var childrenFileNodes = GetFileNodes(node);
            nodes.AddRange(childrenFileNodes);
        }

        return nodes;
    }

    private List<Node> GetDirectoryNodes(Node node)
    {
        var directoryNodes = new List<Node>();

        DirectoryInfo rootDirectoryInfo = new DirectoryInfo(node.Path);

        try
        {
            DirectoryInfo[] subDirectoryInfos = rootDirectoryInfo.GetDirectories();

            foreach (DirectoryInfo directoryInfo in subDirectoryInfos)
            {
                if (!Node.IsHidden(directoryInfo))
                {
                    Node newDirectoryNode = NodeFactory.Create(directoryInfo.FullName, parent: node);
                    directoryNodes.Add(newDirectoryNode);
                }
            }
        }
        catch (UnauthorizedAccessException) { }

        return directoryNodes;
    }

    private List<Node> GetFileNodes(Node node)
    {
        var fileNodes = new List<Node>();

        var parentDirectoryInfo = new DirectoryInfo(node.Path);
        var directoryFileInfos = parentDirectoryInfo.EnumerateFiles();
        foreach (FileInfo fileInfo in directoryFileInfos)
        {
            if (!Node.IsHidden(fileInfo))
            {
                Node newFileNode = NodeFactory.Create(fileInfo.FullName, parent: node);
                fileNodes.Add(newFileNode);
            }
        }

        return fileNodes;
    }
}