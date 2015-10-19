using System.Collections.Generic;

namespace FilesExplorer.Models
{
    public class NodeModel
    {
        public List<Node> GetDrives()
        {
            var drivesList = DirectoryNodes.GetDrives();
            return drivesList;
        }

        public List<Node> GetNodes(string path)
        {
            var nodesList = DirectoryNodes.GetChildren(path);
            return nodesList;
        }
    }
}