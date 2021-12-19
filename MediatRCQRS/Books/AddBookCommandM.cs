using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRCQRS
{
    public record AddBookCommandM(string Title, DateTime ReleaseDate, List<int> AuthorsIDs, string Description) : IRequest<bool>;
}
