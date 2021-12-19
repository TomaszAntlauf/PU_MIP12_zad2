using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using Repository_Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksRepository repo;
        public BooksController(BooksRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("/Wiele ksiag")]
        public IEnumerable<BookDTO> Get([FromQuery]PaginationDTO pagination)
        {
            return this.repo.GetBooks(pagination);
        }

        [HttpGet("/Jedna ksiazka")]
        public BookDTO GetbyId(int Idx)
        {
            return this.repo.GetBookbyId(Idx);
        }

        [HttpPost("/Dodaj ksiazke")]
        public BookDTO Post([FromBody]BookRequestDTO brq)
        {
            return repo.PostBook(brq);
        }

        [HttpDelete("{id}/Usun ksiazke")]
        public void Delete(int id)
        {
            repo.DeleteDTO(id);
        }

        [HttpPost("/Dodaj ocene ksiazce")]
        public void AddBookRate([FromBody] int id, int rate)
        {
            repo.AddBookRate(id, rate);
        }
    }
}
