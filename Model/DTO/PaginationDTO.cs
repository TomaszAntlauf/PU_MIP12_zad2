using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public record PaginationDTO([DefaultValue(0)]int Page, [DefaultValue(2)] int Count);
}
