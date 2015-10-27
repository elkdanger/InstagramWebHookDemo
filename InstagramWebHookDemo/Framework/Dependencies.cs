using System.Web.Http;
using Microsoft.AspNet.WebHooks;

namespace InstagramWebHookDemo.Framework
{
    public class Dependencies
    {
        private static InstagramWebHookClient client;

        public static void Initialise(HttpConfiguration config)
        {
            client = new InstagramWebHookClient(config);
        }

        public static InstagramWebHookClient Client { get { return client; } }

        public static string InstagramAuthToken { get; set; }
    }
}