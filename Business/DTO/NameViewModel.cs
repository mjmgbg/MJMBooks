using System.ComponentModel.DataAnnotations;
using Entities;

namespace Business.DTO
{
    public class NameViewModel : IName
    {
        public int Id { get; set; }

        [Display(Name = "Namn")]
        public string Name { get; set; }
    }
}