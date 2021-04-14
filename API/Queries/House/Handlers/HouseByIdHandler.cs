using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.House.Handlers
{
    public class HouseByIdHandler : IRequestHandler<HouseById, Shared.Models.House>
    {
        private readonly IHouseRepository _repository;

        public HouseByIdHandler(IHouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.House> Handle(HouseById request, CancellationToken cancellationToken)
        {
            Shared.Models.House house = await _repository.GetHouseByIdAsync(request.UserId, request.Id);
            return house;
        }
    }
}