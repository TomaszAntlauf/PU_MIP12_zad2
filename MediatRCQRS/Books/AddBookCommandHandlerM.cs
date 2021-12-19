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
    public class AddBookCommandHandlerM : IRequestHandler<AddBookCommandM, bool>
    {
        private Database db { get; }
        private IElasticClient ec;

        public AddBookCommandHandlerM(Database db, IElasticClient elasticClient)
        {
            this.db = db;
            this.ec = elasticClient;
        }

        public Task<bool> Handle(AddBookCommandM command, CancellationToken token)
        {
            Book book = new Book
            {
                Title = command.Title,
                ReleaseDate = command.ReleaseDate,
                Description =command.Description

            };
            book.Authors = db.Authors.Where(a => command.AuthorsIDs.Contains(a.Id)).ToList();
            db.Books.Add(book);
            db.SaveChanges();

            BookDTO book2 = new BookDTO
            {
                Title = command.Title,
                ReleaseDate = command.ReleaseDate,
                Description = command.Description,
                AvarageRates = 0,
                RatesCount = 0
            };
            book2.Authors = ec.Search<AuthorDTO>(x => x
            .Query(q => q
            .QueryString(qs => qs
            .Fields(x => x
            .Field(f => f.Id))))).Documents.ToList();

            IndexResponse res = ec.IndexDocument<BookDTO>(book2);

            return Task.FromResult(true);
        }
    }
}
