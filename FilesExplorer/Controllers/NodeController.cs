using FilesExplorer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FilesExplorer.Controllers
{
    public class NodeController : ApiController
    {
        // GET: api/Node
        public IEnumerable<Node> Get()
        {
            var nodesList = new NodeModel().GetDrives();
            return nodesList.AsEnumerable();
        }

        // GET: api/Node/
        public IEnumerable<Node> Get(string path)
        {
            var decodedPath = HttpUtility.UrlDecode(path);

            var nodesList = new NodeModel().GetNodes(decodedPath);
            return nodesList.AsEnumerable();
        }

        public IEnumerable<Node> Post(HttpRequestMessage request)
        {
            var data = request.Content.ReadAsStringAsync().Result;

            var decodedPath = HttpUtility.UrlDecode(data);
            var nodesList = new NodeModel().GetNodes(decodedPath);
            return nodesList.AsEnumerable();
        }
    }
}