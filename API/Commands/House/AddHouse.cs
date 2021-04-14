using MediatR;

namespace API.Commands.House
{
    public class AddHouse : IRequest<Shared.Models.House>
    {
        public int UserId { get; set; }
        public Shared.Models.House House { get; set; }

        public AddHouse(int userId, Shared.Models.House house)
        {
            UserId = userId;
            House = house;
        }
    }
}