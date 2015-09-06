using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Business;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [EnableCors("http://localhost:3603,http://mjmbooks.azurewebsites.net,http://books.maaninka.nu", "*", "*")]
    public class PhotoController : ApiController
    {
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
                var message = "Kunde inte ladda upp filen";
                var isUploaded = false;
                var connectionString = ConfigurationManager.ConnectionStrings["storageConnection"].ConnectionString;

                foreach (var file in fileContent.Contents)
                {
                    if (file.Headers.ContentDisposition.Name.Contains("isbn"))
                    {
                        isbn = file.ReadAsStringAsync().Result;
                    }

                    if (file.Headers.ContentType != null)
                    {
                        var stream = file.ReadAsStreamAsync().Result;
                        await Helpers.UploadImageToBlob(connectionString, stream, isbn + ".jpg");
                        isUploaded = true;
                        message = "Filen har laddats upp";
                    }
                }
                var returnMessage = new {isUploaded, message};
                var json = JsonConvert.SerializeObject(returnMessage);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json, Encoding.UTF8, "text/html")
                };
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        //TODO: Should this be implemented?
        // DELETE: api/Photo/5
        //[HttpDelete]
        //[Route("{fileName}")]
        //public async Task<IHttpActionResult> Delete(string fileName)
        //{
        //	//if (!this.photoManager.FileExists(fileName))
        //	//{
        //	//	return NotFound();
        //	//}

        //	//var result = await this.photoManager.Delete(fileName);

        //	//if (result.Successful)
        //	//{
        //	//	return Ok(new { message = result.Message });
        //	//}
        //	//else
        //	//{
        //	//	return BadRequest(result.Message);
        //	//}
        //	return null;
        //}
    }
}