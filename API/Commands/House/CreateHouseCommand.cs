using API.Requests;
using MediatR;

namespace API.Commands.House
{
    public class CreateHouseCommand : IRequest<Models.House>
    {
        public HouseRequest Request { get; set; }
    }
}