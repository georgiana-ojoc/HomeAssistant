using MediatR;

namespace API.Commands.Thermostat
{
    public class AddThermostatCommand : IRequest<Shared.Models.Thermostat>
    {
        public string Email { get; set; }
        public int HouseId { get; set; }
        public int RoomId { get; set; }

        public Shared.Models.Thermostat Thermostat { get; set; }

        public AddThermostatCommand(string email, int houseId, int roomId, Shared.Models.Thermostat thermostat)
        {
            Email = email;
            HouseId = houseId;
            RoomId = roomId;
            Thermostat = thermostat;
        }
    }
}