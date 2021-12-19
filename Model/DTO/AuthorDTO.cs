using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    [ElasticsearchType(IdProperty = nameof(Id))]
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public double AvarageRates { get; set; }
        public int RatesCount { get; set; }
        public List<BookDTO> Books { get; set; }
        [MaxLength(1000)]
        public string CV { get; set; }

    }
}
