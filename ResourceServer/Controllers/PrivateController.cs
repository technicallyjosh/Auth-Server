namespace ResourceServer.Controllers
{
	using System.Web.Http;

	[Authorize]
	public class PrivateController : ApiController
	{
		public IHttpActionResult Get()
		{
			return this.Ok(new
			{
				message = "Your bearer token was good!"
			});
		}
	}
}