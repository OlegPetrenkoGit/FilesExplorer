using FilesExplorer.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FilesExplorer.Controllers
{
    public class DrivesController : ApiController
    {
        public IEnumerable<Node> Get()
        {
            var nodesList = new NodeModel().GetDrives();
            return nodesList.AsEnumerable();
        }
    }
}