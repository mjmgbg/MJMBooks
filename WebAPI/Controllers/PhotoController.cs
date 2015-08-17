using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using WebAPI.Models;

namespace WebAPI.Controllers
{
	[EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net", "*", "*")]
	public class PhotoController : ApiController
	{
		private BlobManager azurePhotoManager;

		public PhotoController()
		//: this(new LocalPhotoManager(HttpRuntime.AppDomainAppPath))
		{
			azurePhotoManager = new BlobManager();
		}

		// GET: api/Photo
		public async Task<IHttpActionResult> Get()
		{
			//	var results = await photoManager.Get();
			//return Ok(new { photos = results });
			return null;
		}

		// POST: api/Photo
		[HttpPost]
		public async Task<HttpResponseMessage> Post()
		{
			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}
			try
			{
				// Read the form data.
				var fileContent = await Request.Content.ReadAsMultipartAsync();
				var isbn = string.Empty;

				foreach (var file in fileContent.Contents)
				{
					if (file.Headers.ContentDisposition.Name.Contains("isbn"))
					{
						isbn = file.ReadAsStringAsync().Result;
					}
					
					if (file.Headers.ContentType!=null)
					{
						var stream = file.ReadAsStreamAsync().Result;
						var photo = await azurePhotoManager.Add(stream, isbn +".jpg");
					
						
					}
				}
				return Request.CreateResponse(HttpStatusCode.OK);
			}
			catch (System.Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
			}
		}

		// DELETE: api/Photo/5
		[HttpDelete]
		[Route("{fileName}")]
		public async Task<IHttpActionResult> Delete(string fileName)
		{
			//if (!this.photoManager.FileExists(fileName))
			//{
			//	return NotFound();
			//}

			//var result = await this.photoManager.Delete(fileName);

			//if (result.Successful)
			//{
			//	return Ok(new { message = result.Message });
			//}
			//else
			//{
			//	return BadRequest(result.Message);
			//}
			return null;
		}
	}
}