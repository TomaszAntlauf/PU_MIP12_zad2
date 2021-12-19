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
    class GetAuthorsQueryHandlerM : IRequestHandler<GetAuthorsQueryM, List<AuthorDTO>>
    {
        private IElasticClient ec;

        public GetAuthorsQueryHandlerM(IElasticClient elasticClient)
        {
            this.ec = elasticClient;
        }

        public Task<List<AuthorDTO>> Handle(GetAuthorsQueryM query, CancellationToken token)
        {
            List<AuthorDTO> listA;
            listA =  ec.Search<AuthorDTO>(x => x.Size(query.Count).Skip(query.Count * query.Page)
            .Query(q=>q
            .MatchAll())).Documents.ToList();

            return Task.FromResult(listA);
        }
    }
}
