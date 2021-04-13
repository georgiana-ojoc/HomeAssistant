using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.House.Handlers
{
    public class HouseByIdHandler : IRequestHandler<HouseById, Models.House>
    {
        private readonly IHouseRepository _repository;

        public HouseByIdHandler(IHouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.House> Handle(HouseById request, CancellationToken cancellationToken)
        {
            Models.House house = await _repository.GetHouseByIdAsync(request.UserId, request.Id);
            return house;
        }
    }
}