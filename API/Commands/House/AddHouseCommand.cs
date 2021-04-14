using MediatR;

namespace API.Commands.House
{
    public class AddHouseCommand : IRequest<Shared.Models.House>
    {
        public string Email { get; set; }
        public Shared.Models.House House { get; set; }

        public AddHouseCommand(string email, Shared.Models.House house)
        {
            Email = email;
            House = house;
        }
    }
}