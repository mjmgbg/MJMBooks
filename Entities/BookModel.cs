using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Entities
{
    public class BookModel : IBaseObject
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImagePath { get; set; }

        public string Isbn { get; set; }

        public int LanguageId { get; set; }

        public DateTime PublishingDate { get; set; }

        public int PublisherId { get; set; }
        public string Description { get; set; }

        public string BgColor { get; set; }

        public string TextColor { get; set; }

        public string TextColorSecond { get; set; }

        public int? SeriesId { get; set; }

        public int? SeriesPartId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsRead { get; set; }

        public virtual LanguageModel Language { get; set; }

        public virtual PublisherModel Publisher { get; set; }

        public virtual SeriesModel Series { get; set; }

        public virtual SeriesPartNumberModel SeriesPart { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthorModel> Authors { get; set; } = new HashSet<AuthorModel>();

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GenreModel> Genres { get; set; } = new HashSet<GenreModel>();

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReaderModel> Readers { get; set; } = new HashSet<ReaderModel>();
    }
}