using System;
using MediatR;

namespace HomeAssistantAPI.Queries.House
{
    public class GetHouseByIdQuery : IRequest<Models.House>
    {
        public Guid Id { get; set; }
    }
}