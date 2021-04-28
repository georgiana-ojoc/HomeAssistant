using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.House.Handlers
{
    public class CreateHouseCommandHandler : Handler, IRequestHandler<CreateHouseCommand, Shared.Models.House>
    {
        private readonly IHouseRepository _repository;

        public CreateHouseCommandHandler(Identity identity, IHouseRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.House> Handle(CreateHouseCommand request, CancellationToken cancellationToken)
        {
            return await _repository.CreateHouseAsync(Identity.Email, new Shared.Models.House
            {
                Name = request.Request.Name
            });
        }
    }
}