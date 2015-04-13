namespace AuthServer.Providers
{
	using System.Collections.Generic;
	using System.Security.Claims;
	using System.Threading.Tasks;

	using Microsoft.Owin.Security;
	using Microsoft.Owin.Security.OAuth;

	using Models;

	public class CustomOAuthProvider : OAuthAuthorizationServerProvider
	{
		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			string clientId;
			string clientSecret;

			if (!context.TryGetBasicCredentials(out clientId, out clientSecret))
			{
				context.TryGetFormCredentials(out clientId, out clientSecret);
			}

			if (context.ClientId == null)
			{
				context.SetError("invalid_clientId", "client_id is not set.");
				return Task.FromResult<object>(null);
			}

			// EXAMPLE CODE: This is an example audience to verify. This should change to be pulled from persistent storage by clientId (db or memory)
			var audience = new Audience();

			// Once you remove the code above and pull from storage, this check simply changes to...
			// if (audience == null)
			if (audience.ClientId != context.ClientId)
			{
				context.SetError("invalid_clientId", string.Format("Invalid client_id '{0}'", context.ClientId));
				return Task.FromResult<object>(null);
			}

			context.Validated();

			return Task.FromResult<object>(null);
		}

		public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {"*"});

			if (string.IsNullOrWhiteSpace(context.UserName) || string.IsNullOrWhiteSpace(context.Password))
			{
				context.SetError("invalid_grant", "Username and password are required.");
				return Task.FromResult<object>(null);
			}

			// TEST CODE: Do db call or something here to validate the username and password
			if (context.UserName != context.Password)
			{
				context.SetError("invalid_grant", "The username or password is incorrect");
				return Task.FromResult<object>(null);
			}

			var identity = new ClaimsIdentity("JWT");

			// Here you can add more claims to your liking
			identity.AddClaims(new List<Claim>
			{
				new Claim(ClaimTypes.Name, context.UserName),
				new Claim("sub", context.UserName),
				new Claim(ClaimTypes.Role, "user"),
				new Claim(ClaimTypes.Email, context.UserName)
			});

			var props = new AuthenticationProperties(new Dictionary<string, string>
			{
				{ "audience", context.ClientId }
			});

			var ticket = new AuthenticationTicket(identity, props);

			context.Validated(ticket);

			return Task.FromResult<object>(null);
		}
	}
}