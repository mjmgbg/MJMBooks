using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Entities;

namespace Business.DTO
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Ange sökväg till bild")]
        [DisplayName("Sökväg till bild")]
        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "ISBN")]
        public string Isbn { get; set; }
        [Display(Name = "Språk")]
        public int LanguageId { get; set; }

        [UIHint("DateTimePicker")]
        [Display(Name = "Utgivningsdatum")]
        public DateTime PublishingDate { get; set; }

        public int PublisherId { get; set; }

        [Required]
        [Display(Name = "Handling")]
        public string Description { get; set; }

        public string BgColor { get; set; }

        public string TextColor { get; set; }

        public string TextColorSecond { get; set; }

        public int? SeriesId { get; set; }

        public int? SeriesPartId { get; set; }
        [Display(Name = "Lades in i systemet")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Uppdaterades")]
        public DateTime UpdateDate { get; set; }
        [Display(Name = "Har läst boken")]
        public bool IsRead { get; set; }
        public virtual NameViewModel Series { get; set; }
        [Display(Name = "Del i serie")]
        public virtual NameViewModel SeriesPart { get; set; }
        public virtual NameViewModel Language { get; set; }
        public virtual NameViewModel Publisher { get; set; }
        [Display(Name = "Författare")]
        public virtual List<PersonViewModel> Authors { get; set; } = new List<PersonViewModel>();
        [Display(Name = "Uppläsare")]
        public virtual ICollection<PersonViewModel> Readers { get; set; } = new List<PersonViewModel>();
        public int[] AuthorsList { get; set; }
        public MultiSelectList AuthorsChoices { get; set; }
        public int[] ReadersList { get; set; }
        public MultiSelectList ReadersChoices { get; set; }
        public SelectList LanguageChoices { get; set; }
        public SelectList PublisherChoices { get; set; }
        public SelectList SeriesChoices { get; set; }
        public SelectList SeriesPartChoices { get; set; }
        public FileUploadView File { get; set; }
        
        private string ConvertNames(List<IPerson> list)
        {
            var temp = string.Empty;
            foreach (var item in list)
            {
                switch (list.Count)
                {
                    case 1:
                        return item.DisplayFullName;
                    case 2:
                        temp += item.LastName + " & ";
                        break;
                }
                if (list.Count > 2)
                {
                    temp += item.LastName + ", ";
                }
            }
            if (list.Count == 2)
            {
                temp = temp.Substring(0, temp.Length - 3);
            }
            if (list.Count > 2)
            {
                temp = temp.Substring(0, temp.Length - 2);
            }

            return temp;
        }

        public string GetReaders()
        {
            var list = new List<IPerson>(Readers);
            return ConvertNames(list);
        }

        public string GetAuthors()
        {
            var list = new List<IPerson>(Authors);
            return ConvertNames(list);
        }
    }
}