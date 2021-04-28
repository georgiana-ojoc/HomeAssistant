using MediatR;
using Shared.Requests;

namespace API.Commands.House
{
    public class CreateHouseCommand : IRequest<Shared.Models.House>
    {
        public HouseRequest Request { get; set; }
    }
}