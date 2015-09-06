using System.Web;

namespace Business.DTO
{
    public class FileUpload
    {
        public HttpPostedFileBase File { get; set; }
    }
}