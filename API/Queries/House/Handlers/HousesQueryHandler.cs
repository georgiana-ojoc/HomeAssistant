using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.House.Handlers
{
    public class HousesQueryHandler: IRequestHandler<HousesQuery,IEnumerable<Models.House>>
    {
        private readonly IHouseRepository _repository;

        public HousesQueryHandler(IHouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.House>> Handle(HousesQuery request, CancellationToken cancellationToken)
        {
            var houses = await _repository.GetHousesAsync(request.Id);
            return houses;
        }
    }
}