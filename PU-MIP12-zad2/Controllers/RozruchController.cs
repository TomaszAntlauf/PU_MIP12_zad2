using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using Repository_Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PU_MIP12_zad2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RozruchController : Controller
    {
        public RozruchController(Repo rp, BooksRepository br, AuthorRepository ar)
        {
            _rp = rp;
            _br = br;
            _ar = ar;
        }

        private Repo _rp { get; set; }
        private BooksRepository _br { get; set; }
        private AuthorRepository _ar { get; set; }

        [HttpGet("Rozruch")]
        public bool Rozruch()
        {
            try {
                if (_rp.ec.Indices.Exists("authors.index").Exists)
                {
                    _rp.ec.Indices.Delete("authors.index");
                }

                if (_rp.ec.Indices.Exists("books.index").Exists)
                {
                    _rp.ec.Indices.Delete("books.index");
                };

                _rp.ec.Indices.Create("authors.index", index => index.Map<AuthorDTO>(x => x.AutoMap()));
                _rp.ec.Indices.Create("books.index", index => index.Map<BookDTO>(x => x.AutoMap()));

                _ar.Db.Authors.RemoveRange(_ar.Db.Authors);
                _br.Db.Books.RemoveRange(_ar.Db.Books); 
                _ar.Db.AuthorRates.RemoveRange(_ar.Db.AuthorRates);
                _br.Db.BookRates.RemoveRange(_ar.Db.BookRates);
                _ar.Db.SaveChanges();
                _br.Db.SaveChanges();

                List<AuthorRequestDTO> listaA = new List<AuthorRequestDTO>();
                List<BookRequestDTO> listaB = new List<BookRequestDTO>();

                for (int a = 1; a <= 10; a++)
                {
                    var tempAuthor = new AuthorRequestDTO
                    {
                        FirstName = "RozruchAutorFName"+a,
                        SecondName = "RozruchAutorSName" + a,
                        CV = "string",
                    };
                    listaA.Add(tempAuthor);
                }

                foreach (var author in listaA)
                {
                    _ar.PostAuthor(author);
                }

                for(int a = 1; a <= 10; a++)
                {
                    _ar.AddAuthorRate(a, _ar.RandomAuhorRate(5));
                    _ar.AddAuthorRate(a, _ar.RandomAuhorRate(5));
                    _ar.AddAuthorRate(a, _ar.RandomAuhorRate(5));
                }

                for (int b = 1; b <= 10; b++)
                {
                    List<int> temp = new List<int>();
                    temp.Add(b-1);
                    var tempBook = new BookRequestDTO
                    {
                        Title = "RozruchKsiazka"+b,
                        Description = "string",
                        ReleaseDate = System.DateTime.Now,
                        AuthorsId = temp
                    };
                    listaB.Add(tempBook);
                }

                foreach (var book in listaB)
                {
                    _br.PostBook(book);
                }

                for (int b = 1; b <= 10; b++)
                {
                    _br.AddBookRate(b, _br.RandomBookRate(5));
                    _br.AddBookRate(b, _br.RandomBookRate(5));
                    _br.AddBookRate(b, _br.RandomBookRate(5));
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
    }

    
}
