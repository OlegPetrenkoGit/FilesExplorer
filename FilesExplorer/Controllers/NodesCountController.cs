using FilesExplorer.Controllers.Utilities;
using FilesExplorer.Models;
using System;
using System.Net.Http;
using System.Web.Http;

namespace FilesExplorer.Controllers
{
    public class NodesCountController : ApiController
    {
        public FileCounters Post(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            Node node = JSON.DeserializeObject<Node>(request);
            var counters = new NodeModel().GetCounters(node);
            return counters;
        }
    }
}