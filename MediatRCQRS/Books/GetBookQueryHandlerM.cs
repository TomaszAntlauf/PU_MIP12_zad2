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
    public class GetBookQueryHandlerM : IRequestHandler<GetBookQueryM, BookDTO>
    {
        private IElasticClient ec;

        public GetBookQueryHandlerM(IElasticClient elasticClient)
        {
            this.ec = elasticClient;
        }

        public Task<BookDTO> Handle(GetBookQueryM query, CancellationToken token)
        {
            return Task.FromResult(ec.Get<BookDTO>(query.id).Source);
        }
    }
}
