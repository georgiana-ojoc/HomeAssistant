using System;
using MediatR;

namespace API.Queries.LightBulb
{
    public class LightBulbByIdQuery : IRequest<Shared.Models.LightBulb>
    {
        public string Email { get; set; }
        public Guid HouseId { get; set; }
        public Guid RoomId { get; set; }
        public Guid Id { get; set; }

        public LightBulbByIdQuery(string email, Guid houseId, Guid roomId, Guid id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}