namespace AuthServer.Formats
{
	using System;
	using System.IdentityModel.Tokens;

	using Microsoft.Owin.Security;
	using Microsoft.Owin.Security.DataHandler.Encoder;

	using Models;

	using Thinktecture.IdentityModel.Tokens;

	public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
	{
		private const string AudiencePropertyKey = "audience";
		private readonly string _issuer;

		public CustomJwtFormat(string issuer)
		{
			this._issuer = issuer;
		}

		public string Protect(AuthenticationTicket data)
		{
			if (data == null)
			{
				throw new ArgumentException("Authentication ticket cannot be null.");
			}

			var audienceId = data.Properties.Dictionary.ContainsKey(AudiencePropertyKey)
				? data.Properties.Dictionary[AudiencePropertyKey]
				: null;

			if (string.IsNullOrWhiteSpace(audienceId))
			{
				throw new InvalidOperationException("AuthenticationTicket.Properties does not include audience.");
			}

			// EXAMPLE CODE: This should be pulling from the persistent store that you are storing the clientIds and secrets
			var audience = new Audience();

			var symmetricKey = audience.Secret;
			var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKey);
			var signingKey = new HmacSigningCredentials(keyByteArray);

			var issued = data.Properties.IssuedUtc;
			var expires = data.Properties.ExpiresUtc;

			if (!issued.HasValue || !expires.HasValue)
			{
				throw new ArgumentException("Authentication ticket must have issued and expires.");
			}

			var token = new JwtSecurityToken(this._issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime,
				expires.Value.UtcDateTime, signingKey);
			var handler = new JwtSecurityTokenHandler();

			return handler.WriteToken(token);
		}

		public AuthenticationTicket Unprotect(string protectedText)
		{
			throw new NotImplementedException();
		}
	}
}