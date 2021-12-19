using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRCQRS
{
    public class AddBookRateCommandHandlerM : IRequestHandler<AddBookRateCommandM, bool>
    {
        private Database db { get; }

        public AddBookRateCommandHandlerM(Database db)
        {
            this.db = db;
        }

        public Task<bool> Handle(AddBookRateCommandM command, CancellationToken token)
        {
            Book des = db.Books.Where(x => x.Id == command.id).FirstOrDefault();

            db.BookRates.Add(new BookRate
            {
                RateType = RateType.BookRate,
                Book = des,
                FkBook = des.Id,
                Date = DateTime.Now,
                Value = (short)command.rate
            });

            db.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
