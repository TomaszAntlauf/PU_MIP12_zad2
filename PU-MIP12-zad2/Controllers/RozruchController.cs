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
        public RozruchController(BooksRepository br, AuthorRepository ar)
        {
            
            _br = br;
            _ar = ar;
        }

        private BooksRepository _br { get; set; }
        private AuthorRepository _ar { get; set; }

        BookRequestDTO bqRqDTO;
        AuthorRequestDTO aqRqDTO;


        [HttpGet("Rozruch")]
        public bool Rozruch()
        {
            try
            {
                if (_br.GetBookbyId(1) == null)
                {
                    if (_ar.GetAuthorbyId(1) == null)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            bqRqDTO = new BookRequestDTO
                            {
                                Title = "Tytuł" + i,
                                ReleaseDate = DateTime.Now,
                                Description = "Opis" + i,
                                AuthorsId = new List<int> { }
                            };
                            aqRqDTO = new AuthorRequestDTO
                            {
                                FirstName = "Autor" + i,
                                SecondName = "AutorNazwisko" + i,
                                CV = "Dorobek" + i,
                                BooksId = new List<int> { }
                            };

                            _br.PostBook(bqRqDTO);
                            _ar.PostAuthor(aqRqDTO);

                            _br.AddBookRate(i, _br.RandomBookRate(5));
                            _br.AddBookRate(i, _br.RandomBookRate(5));
                            _br.AddBookRate(i, _br.RandomBookRate(5));
                            _ar.AddAuthorRate(i, _ar.RandomAuhorRate(5));
                            _ar.AddAuthorRate(i, _ar.RandomAuhorRate(5));
                            _ar.AddAuthorRate(i, _ar.RandomAuhorRate(5));
                        }

                        //bqRqDTO = new BookRequestDTO
                        //{
                        //    Title = "Tytuł" + 1,
                        //    ReleaseDate = DateTime.Now,
                        //    Description = "Opis" + 1,
                        //    AuthorsId = new List<int> {}
                        //};
                        //aqRqDTO = new AuthorRequestDTO
                        //{
                        //    FirstName = "Autor" + 1,
                        //    SecondName = "AutorNazwisko" + 1,
                        //    CV = "Dorobek" + 1,
                        //    BooksId = new List<int>{}
                        //};

                        //_br.PostBook(bqRqDTO);
                        //_ar.PostAuthor(aqRqDTO);

                        //_br.AddBookRate(1, _br.RandomBookRate(5));
                        ////_ar.AddAuthorRate(1, _ar.RandomAuhorRate(5));
                        ////_br.AddBookRate(1, _br.RandomBookRate(5));
                        ////_ar.AddAuthorRate(1, _ar.RandomAuhorRate(5));
                        ////_br.AddBookRate(1, _br.RandomBookRate(5));
                        ////_ar.AddAuthorRate(1, _ar.RandomAuhorRate(5));

                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
    }

    
}
