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
    public class DeleteAuthorCommandHandlerM : IRequestHandler<DeleteAuthorCommandM, bool>
    {
        private readonly Database db;
        private IElasticClient ec;

        public DeleteAuthorCommandHandlerM(Database db, IElasticClient elasticClient)
        {
            this.db = db;
            this.ec = elasticClient;
        }

        public Task<bool> Handle(DeleteAuthorCommandM command, CancellationToken token)
        {
            Author del = db.Authors.Include(x => x.Books).Where(x => x.Id == command.id).FirstOrDefault();
            

            if (del.Books.Any() == false)
            {
                db.Authors.Remove(del);
                db.SaveChanges();
            }

            ec.Delete<AuthorDTO>(command.id);

            return Task.FromResult(true);

        }
    }
}
