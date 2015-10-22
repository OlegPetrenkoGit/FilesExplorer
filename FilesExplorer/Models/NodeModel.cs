using System.Collections.Generic;

namespace FilesExplorer.Models
{
    public class NodeModel
    {
        public List<Node> GetDrives()
        {
            var drivesList = new NodesCollection().GetDrives();
            return drivesList;
        }

        public List<Node> GetNodes(Node node)
        {
            var nodeResponse = new NodesCollection().GetNodes(node);
            return nodeResponse;
        }

        public FileCounters GetCounters(Node node)
        {
            var counters = new FileCounters(node);
            return counters;
        }
    }
}