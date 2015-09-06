using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.DTO
{
    public class IsbnViewModel
    {
        public IsbnViewModel()
        {
            AlreadyInDb = false;
        }

        [Required(ErrorMessage = "Ange ISBN")]
        [MinLength(10, ErrorMessage = "Minst 10 tecken")]
        [DisplayName("ISBN")]
        public string Isbn { get; set; }

        public bool AlreadyInDb { get; set; }
    }
}