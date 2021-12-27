using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using Nest;

namespace Repository_Pattern
{
    public class AuthorRepository
    {
        public Database Db { get; }
        private IElasticClient ec;
        private static Random rdm = new Random();

        public AuthorRepository(Database db, IElasticClient ec)
        {
            Db = db;
            this.ec = ec;
        }

        public List<AuthorDTO> GetAuthors(PaginationDTO pagination)
        {
            return ec.Search<AuthorDTO>(x => x.Size(pagination.Count).Skip(pagination.Count * pagination.Page)).Documents.ToList();
        }

        public AuthorDTO GetAuthorbyId(int Idx)
        {
            return ec.Get<AuthorDTO>(Idx).Source;
        }

        public AuthorDTO PostAuthor(AuthorRequestDTO brq)
        {
            Author aut = new Author
            {
                FirstName = brq.FirstName,
                SecondName = brq.SecondName,
                CV = brq.CV

            };
            aut.Books = Db.Books.Where(a => brq.BooksId.Contains(a.Id)).ToList();
            Db.Authors.Add(aut);
            Db.SaveChanges();

            AuthorDTO autDTO= new AuthorDTO
            {
                Id = aut.Id,
                Books = aut.Books.Select(a => new BookDTO
                {
                    Id = a.Id,
                    Title = a.Title,
                    ReleaseDate = a.ReleaseDate,
                    Description = a.Description
                }).ToList(),
                AvarageRates = 0,
                RatesCount = 0,
                CV = aut.CV,
                FirstName = aut.FirstName,
                SecondName = aut.SecondName
            };

            IndexResponse res = ec.IndexDocument<AuthorDTO>(autDTO);
            
            return autDTO;

        }

        public bool DeleteDTO(int id)
        {
            Author del = Db.Authors.Include(x=>x.Books).FirstOrDefault(x => x.Id == id);

            if (del.Books.Any())
            {
                return false;
            }

            Db.Authors.Remove(del);
            Db.SaveChanges();

            ec.Delete<AuthorDTO>(id);

            return true;

        }

        public void AddAuthorRate(int id, int rate)
        {
            Author des = Db.Authors.Where(x => x.Id == id).FirstOrDefault();

            Db.AuthorRates.Add(new AuthorRate
            {
                RateType = RateType.AuthorRate,
                Author = des,
                FkAuthor = des.Id,
                Date = DateTime.Now,
                Value = (short)rate
            });

            Db.SaveChanges();

            des = Db.Authors.Include(x=>x.Rates).Where(x => x.Id == id).FirstOrDefault();

            ec.Update<AuthorDTO>(id, u => u
            .Doc(new AuthorDTO
            {
                Id=des.Id,
                FirstName=des.FirstName,
                SecondName=des.SecondName,
                CV=des.CV,
                AvarageRates = des.Rates.Average(r => r.Value),
                RatesCount = des.Rates.Count()
            }));

        }

        public int RandomAuhorRate(int i)
        {
            return rdm.Next(i);
        }
    }
}
