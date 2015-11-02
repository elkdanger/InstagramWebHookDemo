using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace InstagramWebHookDemo.Hubs
{
    [HubName("imageHub")]
    public class InstagramImageHub : Hub
    {
        public void ShowImage(string imageUri, string user, string caption)
        {
            Clients.All.showImage(imageUri, user, caption);
        }
    }
}