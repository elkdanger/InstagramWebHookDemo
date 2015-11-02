using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstagramWebHookDemo.Hubs;
using InstagramWebHookDemo.Models;
using InstaSharp.Endpoints;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.WebHooks;

namespace InstagramWebHookDemo.Framework
{
    public class InstagramWebHookHandler : WebHookHandler
    {
        private static Lazy<IHubContext> hubContext = new Lazy<IHubContext>(() =>
            GlobalHost.ConnectionManager.GetHubContext<InstagramImageHub>());

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

            var media = new Tags(config, new InstaSharp.Models.Responses.OAuthResponse
            {
                AccessToken = user.InstagramAccessToken,
                User = new InstaSharp.Models.UserInfo
                {
                    Username = "elkdanger"
                }
            });

            var result = await media.Recent(notifications.First().ObjectId);

            foreach (var image in result.Data)
            {
                hubContext.Value.Clients.All.showImage(image.Images.LowResolution, image.User.Username, image.Caption.Text);
            }
            
            return;
        }
    }
}