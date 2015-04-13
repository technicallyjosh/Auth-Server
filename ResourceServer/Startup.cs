namespace ResourceServer
{
	using System.Web.Configuration;
	using System.Web.Http;

	using Microsoft.Owin.Security.DataHandler.Encoder;
	using Microsoft.Owin.Security.Jwt;

	using Owin;

	using AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode;

	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var config = new HttpConfiguration();
			WebApiConfig.Register(config);

			// These were used to sign the token by the auth server
			var issuer = WebConfigurationManager.AppSettings["AuthIssuer"];
			var audience = WebConfigurationManager.AppSettings["AuthClientId"];
			var symmetricKey = TextEncodings.Base64Url.Decode(WebConfigurationManager.AppSettings["AuthSecret"]);

			app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
			{
				AuthenticationMode = AuthenticationMode.Active,
				AllowedAudiences = new[] { audience },
				IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
				{
					new SymmetricKeyIssuerSecurityTokenProvider(issuer, symmetricKey)
				}
			});

			app.UseWebApi(config);
		}
	}
}