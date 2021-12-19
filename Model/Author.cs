 using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public List<Book> Books { get; set; }
        public List<AuthorRate> Rates { get; set; }
        [MaxLength(1000)]
        public string CV { get; set; }
    }
}
