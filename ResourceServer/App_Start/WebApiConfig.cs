namespace ResourceServer
{
	using System.Linq;
	using System.Net.Http.Formatting;
	using System.Web.Http;

	using Newtonsoft.Json.Serialization;

	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });

			// Remove XML formatter to default to json
			config.Formatters.Remove(config.Formatters.XmlFormatter);
			
			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
		}
	}
}