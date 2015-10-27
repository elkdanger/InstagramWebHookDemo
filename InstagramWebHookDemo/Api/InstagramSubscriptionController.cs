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
            var sub = await client.SubscribeAsync(string.Empty, Url, "awesome");

            return Ok(sub);
        }

        public async Task UnsubscribeAll()
        {
            var client = Dependencies.Client;

            await client.UnsubscribeAsync(string.Empty);
        }
    }
}
