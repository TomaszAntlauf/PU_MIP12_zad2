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
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorRepository repo;
        public AuthorsController(AuthorRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet("/Wiele autorow")]
        public IEnumerable<AuthorDTO> Get([FromQuery] PaginationDTO pagination)
        {
            return this.repo.GetAuthors(pagination);
        }

        [HttpGet("/Jeden autor")]
        public AuthorDTO GetbyId(int Idx)
        {
            return this.repo.GetAuthorbyId(Idx);
        }

        [HttpPost("/Dodaj autora")]
        public AuthorDTO Post([FromBody] AuthorRequestDTO brq)
        {
            return repo.PostAuthor(brq);
        }

        [HttpDelete("{id}/Usun autora")]
        public void Delete(int id)
        {
            repo.DeleteDTO(id);
        }

        [HttpPost("/Dodaj ocene autorowi")]
        public void AddAuthorRate([FromBody] int id, int rate)
        {
            repo.AddAuthorRate(id, rate);
        }
    }
}
