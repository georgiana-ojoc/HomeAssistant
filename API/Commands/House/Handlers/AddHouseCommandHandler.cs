using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.House.Handlers
{
    public class AddHouseCommandHandler : IRequestHandler<AddHouseCommand, Shared.Models.House>
    {
        private readonly IHouseRepository _repository;

        public AddHouseCommandHandler(IHouseRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.House> Handle(AddHouseCommand request, CancellationToken cancellationToken)
        {
            return await _repository.CreateHouseAsync(request.Email, request.House);
        }
    }
}