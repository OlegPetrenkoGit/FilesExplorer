using FilesExplorer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FilesExplorer.Controllers
{
    public class NodeController : ApiController
    {
        // GET: api/Node
        public IEnumerable<Node> Get()
        {
            var nodesList = new NodeModel().GetNodes(@"c:\");
            return nodesList.AsEnumerable();
        }

        // GET: api/Node/c:\
        public IEnumerable<Node> Get(string path)
        {
            var nodesList = new NodeModel().GetNodes(path);
            return nodesList.AsEnumerable();
        }

        // POST: api/Product
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Product/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Product/5
        public void Delete(int id)
        {
        }
    }
}