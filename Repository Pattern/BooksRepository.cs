using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using Nest;

namespace Repository_Pattern
{
    public class BooksRepository
    {
        public Database Db { get; }
        private IElasticClient ec;
        private static Random rdm = new Random();
        public BooksRepository(Database db, IElasticClient ec)
        {
            Db = db;
            this.ec = ec;
        }

        public int RandomBookRate(int i)
        {
            return rdm.Next(i);
        }
        public List<BookDTO> GetBooks(PaginationDTO pagination)
        {
            return ec.Search<BookDTO>(x => x.Size(pagination.Count).Skip(pagination.Count * pagination.Page)
            .Query(q => q
            .MatchAll())).Documents.ToList();
        }

        public BookDTO GetBookbyId(int Idx)
        {
            return ec.Get<BookDTO>(Idx).Source;
        }

        public BookDTO PostBook(BookRequestDTO brq)
        {
            Book book = new Book
            {
                Title = brq.Title,
                ReleaseDate = brq.ReleaseDate,
                Description = brq.Description
            };
            book.Authors = Db.Authors.Where(a => brq.AuthorsId.Contains(a.Id)).ToList();
            //book.Rates = null;
            Db.Books.Add(book);
            Db.SaveChanges();

            BookDTO bookDTO = new BookDTO
            {
                Id = book.Id,
                Authors = book.Authors.Select(a => new AuthorDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    SecondName = a.SecondName,
                    CV=a.CV
                }).ToList(),
                AvarageRates = 0,
                RatesCount = 0,
                Description = book.Description,
                ReleaseDate = book.ReleaseDate,
                Title = book.Title
            };

            IndexResponse res = ec.IndexDocument<BookDTO>(bookDTO);

            return bookDTO;

        }

        public bool DeleteDTO(int id)
        {
            Book del = Db.Books.Where(x => x.Id == id).FirstOrDefault();

            Db.Books.Remove(del);
            Db.SaveChanges();

            ec.Delete<BookDTO>(id);

            return true;
        }

        public void AddBookRate(int id, int rate)
        {
            Book des = Db.Books.Where(x => x.Id == id).FirstOrDefault();

            Db.BookRates.Add(new BookRate
            {
                RateType = RateType.BookRate,
                Book = des,
                FkBook = des.Id,
                Date = DateTime.Now,
                Value = (short)rate
            });

            Db.SaveChanges();

            des = Db.Books.Where(x => x.Id == id).FirstOrDefault();

            ec.Update<BookDTO>(id, u => u
            .Doc(new BookDTO
            {
                AvarageRates=des.Rates.Average(r => r.Value),
                RatesCount=des.Rates.Count()
            })) ;
            
               
        }


    }
}
