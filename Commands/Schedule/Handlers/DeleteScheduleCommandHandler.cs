using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.Schedule.Handlers
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