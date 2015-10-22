using FilesExplorer.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace FilesExplorer.Controllers
{
    public class NodesController : ApiController
    {
        public List<Node> Post(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            Node node = DeserializeObject<Node>(request);
            var nodes = new NodeModel().GetNodes(node);
            return nodes;
        }

        private T DeserializeObject<T>(HttpRequestMessage request)
        {
            var data = request.Content.ReadAsStringAsync().Result;
            return new JavaScriptSerializer().Deserialize<T>(data);
        }
    }
}