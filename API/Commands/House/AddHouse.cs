using MediatR;

namespace API.Commands.House
{
    public class AddHouse : IRequest<Models.House>
    {
        public int UserId { get; set; }
        public Models.House House { get; set; }

        public AddHouse(int userId, Models.House house)
        {
            UserId = userId;
            House = house;
        }
    }
}