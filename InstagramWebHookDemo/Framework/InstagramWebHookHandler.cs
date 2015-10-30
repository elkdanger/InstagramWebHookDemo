using System.Collections.Generic;
using System.Threading.Tasks;
using InstagramWebHookDemo.Models;
using Microsoft.AspNet.WebHooks;
using InstaSharp;
using System.Linq;

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

            var repo = new UserRepository();
            var user = await repo.GetUser("steve");
            var config = Dependencies.GetConfig(context.Request.RequestUri);

            var media = new InstaSharp.Endpoints.Tags(config, new InstaSharp.Models.Responses.OAuthResponse
            {
                AccessToken = user.InstagramAccessToken,
                User = new InstaSharp.Models.UserInfo
                {
                    Username = "elkdanger"
                }
            });

            var result = await media.Recent(notifications.First().ObjectId);
            
            return;
        }
    }
}