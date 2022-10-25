using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth;
using Newtonsoft.Json;
using BFuel.Droid.Renderers;
using BFuel.PaginasInicio;
using BFuel.Servicos;
using BFuel.BFDomain.Models;

[assembly:ExportRenderer(typeof(LoginFacebookPage),typeof(LoginOAuthRenderer))]
namespace BFuel.Droid.Renderers
{
    public class LoginOAuthRenderer : PageRenderer
    {
        RegisterService _service;

        [Obsolete]
        public LoginOAuthRenderer()
        {
            var oauth = new OAuth2Authenticator
            (
                clientId: "802324417755735",
                scope: "email",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html")
            );

            oauth.Completed += async (sender, args) =>
            {
                if (args.IsAuthenticated)
                {
                    var token = args.Account.Properties["access_token"].ToString();

                    var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me?fields=id,name,email"), null, args.Account);
                    var response = await request.GetResponseAsync();

                    dynamic obj = JsonConvert.DeserializeObject(response.GetResponseText());

                    var id = obj.id.ToString();
                    var name = obj.name.ToString();
                    var email = obj.email.ToString();

                    _service = new RegisterService();

                    await _service.Register(name, email, id, (int)UserTypes.Types.Facebook);

                    if (!_service.IsSucessful)
                        Redirect();
                }
                else
                {
                    Redirect();
                }
            };

            var activity = this.Context as Activity;

            activity.StartActivity(oauth.GetUI(activity));
        }

        public void Redirect()
        {
            App.GoToMain("Erro");
        }
    }
}