using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.House.Handlers
{
    public class DeleteHouseHandler : IRequestHandler<DeleteHouse, Models.House>
    {
        private readonly IHouseRepository _repository;

        public DeleteHouseHandler(IHouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Models.House> Handle(DeleteHouse request, CancellationToken cancellationToken)
        {
            Models.House house = await _repository.DeleteHouse(request.UserId, request.Id);
            return house;
        }
    }
}