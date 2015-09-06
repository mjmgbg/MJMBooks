using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Entities
{
    public class AuthorModel : IPerson
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayFullName => FirstName + " " + LastName;

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BookModel> Books { get; set; } = new HashSet<BookModel>();
    }
}