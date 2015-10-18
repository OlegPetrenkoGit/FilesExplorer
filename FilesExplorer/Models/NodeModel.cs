using System.Collections.Generic;

namespace FilesExplorer.Models
{
    public class NodeModel
    {
        public List<Node> GetNodes(string path)
        {
            var nodesList = new TreeNode().Nodes;
            return nodesList;
        }
    }
}