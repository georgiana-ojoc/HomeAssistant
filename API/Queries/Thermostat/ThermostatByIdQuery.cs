using MediatR;

namespace API.Queries.Thermostat
{
    public class ThermostatByIdQuery : IRequest<Shared.Models.Thermostat>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public ThermostatByIdQuery(string email, int houseId, int roomId, int id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}