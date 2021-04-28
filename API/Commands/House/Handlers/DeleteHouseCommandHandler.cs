using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.House.Handlers
{
    public class DeleteHouseCommandHandler : Handler, IRequestHandler<DeleteHouseCommand, Shared.Models.House>
    {
        private readonly IHouseRepository _repository;

        public DeleteHouseCommandHandler(Identity identity, IHouseRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.House> Handle(DeleteHouseCommand request, CancellationToken cancellationToken)
        {
            return await _repository.DeleteHouseAsync(Identity.Email, request.Id);
        }
    }
}