using System;
using System.Configuration;
using System.Web.Http;
using InstaSharp;
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

        public static InstagramConfig GetConfig(Uri requestUri)
        {
            var url = requestUri.Scheme + System.Uri.SchemeDelimiter + requestUri.Host +
                (requestUri.IsDefaultPort ? "" : ":" + requestUri.Port);

            var config = new InstagramConfig(
                ConfigurationManager.AppSettings["MS_WebHookReceiverSecret_InstagramId"],
                ConfigurationManager.AppSettings["MS_WebHookReceiverSecret_Instagram"],
                $"{url}/home/loginCallback");

            return config;
        }
    }
}