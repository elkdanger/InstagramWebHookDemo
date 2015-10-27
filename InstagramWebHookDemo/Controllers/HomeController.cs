using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using InstaSharp;

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
            var config = GetConfig();

            var scopes = new List<OAuth.Scope>();
            scopes.Add(InstaSharp.OAuth.Scope.Likes);
            scopes.Add(InstaSharp.OAuth.Scope.Comments);

            var link = InstaSharp.OAuth.AuthLink(config.OAuthUri + "authorize", config.ClientId, config.RedirectUri, scopes, InstaSharp.OAuth.ResponseType.Code);

            return Redirect(link);
        }

        public async Task<ActionResult> LoginCallback(string code)
        {
            var config = GetConfig();
            var auth = new OAuth(config);
            var oauthResponse = await auth.RequestToken(code);

            FormsAuthentication.SetAuthCookie("steve", false);

            Response.Cookies.Add(new HttpCookie("insta_auth", oauthResponse.AccessToken));

            return RedirectToAction("index");
        }

        private InstagramConfig GetConfig()
        {
            var url = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

            var config = new InstagramConfig(
                ConfigurationManager.AppSettings["MS_WebHookReceiverSecret_InstagramId"],
                ConfigurationManager.AppSettings["MS_WebHookReceiverSecret_Instagram"],
                $"{url}/home/loginCallback");

            return config;
        }
    }
}