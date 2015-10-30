using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using InstagramWebHookDemo.Framework;
using InstagramWebHookDemo.Models;

namespace InstagramWebHookDemo.Api
{
    [RoutePrefix("api/instagram")]
    public class InstagramSubscriptionController : ApiController
    {
        [Route("subscribe")]
        [Authorize]
        public async Task<IHttpActionResult> PostSubscribe(SubscribeRequest request)
        {
            if (request == null)
                return this.BadRequest("You must send a tag request");

            if (string.IsNullOrWhiteSpace(request.Tags))
                return this.BadRequest("You must specify a tag");
            
            var client = Dependencies.Client;

            var tasks = request.Tags.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(tag => Task.Run(() => client.SubscribeAsync(string.Empty, Url, tag)));

            var subscriptions = await Task.WhenAll(tasks);

            return Ok(subscriptions);
        }

        public async Task UnsubscribeAll()
        {
            var client = Dependencies.Client;

            await client.UnsubscribeAsync(string.Empty);
        }
    }
}
