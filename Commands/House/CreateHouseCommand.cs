using HomeAssistantAPI.Requests;
using MediatR;

namespace HomeAssistantAPI.Commands.House
{
    public class CreateHouseCommand : IRequest<Models.House>
    {
        public HouseRequest Request { get; set; }
    }
}