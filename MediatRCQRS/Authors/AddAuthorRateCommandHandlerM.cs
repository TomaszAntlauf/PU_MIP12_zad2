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
    public class AddAuthorRateCommandHandlerM : IRequestHandler<AddAuthorRateCommandM, bool>
    {
        private Database db { get; }

        public AddAuthorRateCommandHandlerM(Database db)
        {
            this.db = db;
        }

        public Task<bool> Handle(AddAuthorRateCommandM command, CancellationToken token)
        {
            Author des = db.Authors.Where(x => x.Id == command.id).FirstOrDefault();

            db.AuthorRates.Add(new AuthorRate
            {
                RateType = RateType.AuthorRate,
                Author = des,
                FkAuthor = des.Id,
                Date = DateTime.Now,
                Value = (short)command.rate
            });

            db.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
