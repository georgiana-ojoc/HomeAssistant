using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Schedule.Handlers
{
    public class DeleteScheduleCommandHandler : Handler, IRequestHandler<DeleteScheduleCommand, Models.Schedule>
    {
        private readonly IScheduleRepository _repository;

        public DeleteScheduleCommandHandler(Identity identity, IScheduleRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Schedule> Handle(DeleteScheduleCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.DeleteScheduleAsync(Identity.Email, request.Id);
        }
    }
}