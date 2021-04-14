using MediatR;

namespace API.Commands.Thermostat
{
    public class DeleteThermostatCommand : IRequest<Shared.Models.Thermostat>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }
        public int Id { get; set; }

        public DeleteThermostatCommand(string email, int houseId, int roomId, int id)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Id = id;
        }
    }
}