using MediatR;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRCQRS
{
    public class GetBooksQueryHandlerM : IRequestHandler<GetBooksQueryM,List<BookDTO>>
    {
        private IElasticClient ec;

        public GetBooksQueryHandlerM(IElasticClient elasticClient)
        {
            this.ec = elasticClient;
        }

        public Task<List<BookDTO>> Handle(GetBooksQueryM query, CancellationToken token)
        {
            List<BookDTO> listA;
            listA = ec.Search<BookDTO>(x => x.Size(query.Count).Skip(query.Count * query.Page)
           .Query(q => q
           .QueryString(qs => qs
           .Fields(x => x
           .Field(f => f.Id))))).Documents.ToList();

            return Task.FromResult(listA);
        }

    }
}
