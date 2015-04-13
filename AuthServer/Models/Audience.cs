namespace AuthServer.Models
{
	using System.ComponentModel.DataAnnotations;
	using System.Web.Configuration;

	public class Audience
	{
		public Audience()
		{
			this.Name = "My API";
			this.ClientId = WebConfigurationManager.AppSettings["AuthClientId"];
			this.Secret = WebConfigurationManager.AppSettings["AuthSecret"];
		}

		[MaxLength(100)]
		[Required]
		public string Name { get; set; }

		[MaxLength(32)]
		public string ClientId { get; set; }

		[Required]
		[MaxLength(80)]
		public string Secret { get; set; }
	}
}