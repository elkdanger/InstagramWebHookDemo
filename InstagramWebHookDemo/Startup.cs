using Owin;

namespace InstagramWebHookDemo
{
    public class Startup
    {
        public void Configure(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}