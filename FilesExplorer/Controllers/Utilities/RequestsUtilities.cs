using System.Net.Http;
using System.Web.Script.Serialization;

namespace FilesExplorer.Controllers.Utilities
{
    public static class JSON
    {
        public static T DeserializeObject<T>(HttpRequestMessage request)
        {
            var data = request.Content.ReadAsStringAsync().Result;
            return new JavaScriptSerializer().Deserialize<T>(data);
        }
    }
}