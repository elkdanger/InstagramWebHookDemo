using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InstagramWebHookDemo.Framework;
using InstagramWebHookDemo.Models;
using InstaSharp;
using MongoDB.Driver;

namespace InstagramWebHookDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly InstagramConfig config;

        public HomeController()
        {
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var config = Dependencies.GetConfig(Request.Url);

            var scopes = new List<OAuth.Scope>();
            scopes.Add(InstaSharp.OAuth.Scope.Likes);
            scopes.Add(InstaSharp.OAuth.Scope.Comments);

            var link = InstaSharp.OAuth.AuthLink(config.OAuthUri + "authorize", config.ClientId, config.RedirectUri, scopes, InstaSharp.OAuth.ResponseType.Code);

            return Redirect(link);
        }

        public async Task<ActionResult> LoginCallback(string code)
        {
            var config = Dependencies.GetConfig(Request.Url);
            var auth = new OAuth(config);
            var oauthResponse = await auth.RequestToken(code);

            FormsAuthentication.SetAuthCookie("steve", false);

            // Save the token to Mongo
            var repo = new UserRepository();
            var user = await repo.GetUser("steve");

            if (user == null)
            {
                user = new UserInfo
                {
                    Username = "steve",
                    InstagramAccessToken = oauthResponse.AccessToken
                };

                await repo.InsertUser(user);
            }
            else
            {
                var update = Builders<UserInfo>.Update
                    .Set(u => u.InstagramAccessToken, oauthResponse.AccessToken);

                await repo.Collection.UpdateOneAsync(u => u.Username == "steve", update);
            }

            Dependencies.InstagramAuthToken = oauthResponse.AccessToken;

            return RedirectToAction("index");
        }        
    }
}