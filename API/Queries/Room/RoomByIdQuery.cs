using System;
using MediatR;

namespace API.Queries.Room
{
    public class RoomByIdQuery : IRequest<Shared.Models.Room>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid Id { get; set; }

        public RoomByIdQuery(string email, Guid houseId, Guid id)
        {
            Email = email;
            HouseId = houseId;
            Id = id;
        }
    }
}