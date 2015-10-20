using FilesExplorer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace FilesExplorer.Controllers
{
    public class NodeController : ApiController
    {
        public IEnumerable<Node> Get()
        {
            var nodesList = new NodeModel().GetDrives();
            return nodesList.AsEnumerable();
        }

        public IEnumerable<Node> Post(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            Node node = DeserializeObject<Node>(request);
            var nodesList = new NodeModel().GetNodes(node);
            return nodesList.AsEnumerable();
        }

        private T DeserializeObject<T>(HttpRequestMessage request)
        {
            var data = request.Content.ReadAsStringAsync().Result;
            return new JavaScriptSerializer().Deserialize<T>(data);
        }
    }
}