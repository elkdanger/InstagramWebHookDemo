using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.WebHooks;

namespace InstagramWebHookDemo.Framework
{
    public class InstagramWebHookHandler : WebHookHandler
    {
        public InstagramWebHookHandler()
        {
            this.Receiver = "instagram";
        }

        public override async Task ExecuteAsync(string receiver, WebHookHandlerContext context)
        {
            var client = Dependencies.Client;
            var notifications = context.GetDataOrDefault<IEnumerable<InstagramNotification>>();

            return;
        }
    }
}