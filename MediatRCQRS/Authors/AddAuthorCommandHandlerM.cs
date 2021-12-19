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
    public class AddAuthorCommandHandlerM : IRequestHandler<AddAuthorCommandM, bool>
    {
        private Database db { get; }
        private IElasticClient ec;

        public AddAuthorCommandHandlerM(Database db,IElasticClient elasticClient)
        {
            this.db = db;
            this.ec = elasticClient;
        }

        public Task<bool> Handle(AddAuthorCommandM command, CancellationToken token)
        {
            Author aut = new Author
            {
                FirstName = command.FirstName,
                SecondName = command.SecondName,
                CV = command.CV

            };
            aut.Books = db.Books.Where(a => command.BooksIDs.Contains(a.Id)).ToList();
            db.Authors.Add(aut);
            db.SaveChanges();

            AuthorDTO aut2 = new AuthorDTO
            {
                FirstName = command.FirstName,
                SecondName = command.SecondName,
                CV = command.CV,
                AvarageRates = 0,
                RatesCount = 0,

            };
            aut2.Books = ec.Search<BookDTO>(x=>x
            .Query(q=>q
            .QueryString(qs=>qs
            .Fields(x=>x
            .Field(f=>f.Id))))).Documents.ToList();


            IndexResponse res = ec.IndexDocument<AuthorDTO>(aut2);

            return Task.FromResult(true);
        }

    }
}
