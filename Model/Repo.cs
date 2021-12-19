using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
    public class Repo
    {
        public IElasticClient ec { get; set; }

        public Repo(string url)
        {
            ec = new ElasticClient(new Elasticconection(new Uri(url)));
        }
    }
}
