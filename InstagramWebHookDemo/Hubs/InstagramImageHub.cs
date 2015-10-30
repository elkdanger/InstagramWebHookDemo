using Microsoft.AspNet.SignalR;

namespace InstagramWebHookDemo.Hubs
{
    public class InstagramImageHub : Hub
    {
        public void ShowImage(string imageUri, string user, string caption)
        {
            Clients.All.showImage(imageUri, user, caption);
        }
    }
}