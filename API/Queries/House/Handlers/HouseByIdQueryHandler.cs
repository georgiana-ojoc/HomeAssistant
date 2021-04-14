using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.House.Handlers
{
    public class HouseByIdQueryHandler : IRequestHandler<HouseByIdQuery, Shared.Models.House>
    {
        private readonly IHouseRepository _repository;

        public HouseByIdQueryHandler(IHouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.House> Handle(HouseByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetHouseByIdAsync(request.Email, request.Id);
        }
    }
}