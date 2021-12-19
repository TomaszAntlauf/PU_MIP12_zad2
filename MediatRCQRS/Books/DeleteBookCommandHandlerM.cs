using MediatR;
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
    public class DeleteBookCommandHandlerM : IRequestHandler<DeleteBookCommandM, bool>
    {
        private readonly Database db;
        private IElasticClient ec;

        public DeleteBookCommandHandlerM(Database db, IElasticClient elasticClient)
        {
            this.db = db;
            this.ec = elasticClient;
        }

        public Task<bool> Handle(DeleteBookCommandM command, CancellationToken token)
        {
            Book del = db.Books.Where(x => x.Id == command.id).FirstOrDefault();

            db.Books.Remove(del);
            db.SaveChanges();

            ec.Delete<BookDTO>(command.id);

            return Task.FromResult(true);
        }
    }
}
