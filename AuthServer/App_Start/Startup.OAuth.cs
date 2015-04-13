namespace AuthServer
{
	using System;
	using System.Web.Configuration;

	using Formats;

	using Microsoft.Owin;
	using Microsoft.Owin.Security.OAuth;

	using Owin;

	using Providers;

	public partial class Startup
	{
		public void ConfigureOAuth(IAppBuilder app)
		{
			var endpoint = new PathString("/oauth2/token");
			var expireInMinutes = int.Parse(WebConfigurationManager.AppSettings["AuthTokenExpireInMinutes"]);
			var issuer = WebConfigurationManager.AppSettings["AuthIssuer"];

			var oAuthServerOptions = new OAuthAuthorizationServerOptions
			{
				// You shouldn't be deploying DEBUG mode to production ;)
#if DEBUG
				AllowInsecureHttp = true,
#endif
				TokenEndpointPath = endpoint,
				AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(expireInMinutes),
				Provider = new CustomOAuthProvider(),
				AccessTokenFormat = new CustomJwtFormat(issuer)
			};

			// Middleware for wiring up the auth server
			app.UseOAuthAuthorizationServer(oAuthServerOptions);
		}
	}
}