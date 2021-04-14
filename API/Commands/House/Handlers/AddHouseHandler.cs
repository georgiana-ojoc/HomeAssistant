using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.House.Handlers
{
    public class AddHouseHandler : IRequestHandler<AddHouse, Shared.Models.House>
    {
        private readonly IHouseRepository _repository;

        public AddHouseHandler(IHouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.House> Handle(AddHouse request, CancellationToken cancellationToken)
        {
            Shared.Models.House house = await _repository.CreateHouseAsync(request.UserId, request.House);
            return house;
        }
    }
}