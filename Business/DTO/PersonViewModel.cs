using System.ComponentModel.DataAnnotations;
using Entities;

namespace Business.DTO
{
    public class PersonViewModel : IPerson
    {
        public int Id { get; set; }

        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Display(Name = "Författare")]
        public string DisplayFullName => FirstName + " " + LastName;
    }
}