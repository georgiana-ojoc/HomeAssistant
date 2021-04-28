using System;
using MediatR;

namespace API.Queries.House
{
    public class GetHouseByIdQuery : IRequest<Shared.Models.House>
    {
        public Guid Id { get; set; }
    }
}