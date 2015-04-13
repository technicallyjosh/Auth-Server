namespace ResourceServer.Controllers
{
	using System.Web.Http;

	public class PublicController : ApiController
	{
		public IHttpActionResult Get()
		{
			return this.Ok(new
			{
				message = "You don't need a bearer token to access this route!"
			});
		}
	}
}