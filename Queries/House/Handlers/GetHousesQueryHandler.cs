using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.House.Handlers
{
    public class GetHousesQueryHandler : Handler, IRequestHandler<GetHousesQuery, IEnumerable<Models.House>>
    {
        private readonly IHouseRepository _repository;

        public GetHousesQueryHandler(Identity identity, IHouseRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.House>> Handle(GetHousesQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetHousesAsync(Identity.Email);
        }
    }
}