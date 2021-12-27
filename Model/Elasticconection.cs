using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model
{
    public class Elasticconection : ConnectionSettings
    {
        public Elasticconection(Uri uri = null) : base(uri)
        {
            this.DefaultMappingFor<BookDTO>(x => x.IndexName("ksiazka"));
            this.DefaultMappingFor<AuthorDTO>(x => x.IndexName("autor"));
        }
    }
}