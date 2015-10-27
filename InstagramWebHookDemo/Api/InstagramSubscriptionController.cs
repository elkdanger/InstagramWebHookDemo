using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using InstagramWebHookDemo.Framework;

namespace InstagramWebHookDemo.Api
{
    [RoutePrefix("api/instagram")]
    public class InstagramSubscriptionController : ApiController
    {
        [Route("subscribe")]
        public async Task<IHttpActionResult> PostSubscribe()
        {
            var client = Dependencies.Client;
            System.Net.Http.Headers.CookieState authCookie = GetAuthCookie();

            if (authCookie != null)
            {
                var sub = await client.SubscribeAsync(authCookie.Value, Url, "awesome");

                return Ok(sub);
            }

            return Unauthorized();
        }

        public async Task UnsubscribeAll()
        {
            var client = Dependencies.Client;
            System.Net.Http.Headers.CookieState authCookie = GetAuthCookie();

            await client.UnsubscribeAsync(authCookie.Value);
        }

        private System.Net.Http.Headers.CookieState GetAuthCookie()
        {
            return Request.Headers.GetCookies().FirstOrDefault()["insta_auth"];
        }
    }
}
