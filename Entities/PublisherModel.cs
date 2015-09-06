using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Entities
{
    public class PublisherModel : IName
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookModel> Books { get; set; } = new HashSet<BookModel>();
    }
}