namespace AuthServer
{
	using System.Web.Http;

	using Microsoft.Owin.Cors;

	using Owin;

	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			this.ConfigureOAuth(app);

			var config = new HttpConfiguration();
			WebApiConfig.Register(config);

			// For demonstration purposes, we allow all. You could add whitelisted domains here if you wanted
			app.UseCors(CorsOptions.AllowAll);

			app.UseWebApi(config);
		}
	}
}