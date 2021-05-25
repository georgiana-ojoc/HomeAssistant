using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.House.Handlers
{
    public class DeleteHouseCommandHandler : Handler, IRequestHandler<DeleteHouseCommand, Models.House>
    {
        private readonly IHouseRepository _repository;

        public DeleteHouseCommandHandler(Identity identity, IHouseRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.House> Handle(DeleteHouseCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteHouseAsync(Identity.Email, request.Id);
        }
    }
}