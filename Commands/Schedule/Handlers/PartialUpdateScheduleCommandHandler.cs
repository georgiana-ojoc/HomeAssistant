using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.Schedule.Handlers
{
    public class PartialUpdateScheduleCommandHandler : Handler,
        IRequestHandler<PartialUpdateScheduleCommand, Models.Schedule>
    {
        private readonly IScheduleRepository _repository;

        public PartialUpdateScheduleCommandHandler(Identity identity, IScheduleRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Schedule> Handle(PartialUpdateScheduleCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateScheduleAsync(Identity.Email, request.Id, request.Patch);
        }
    }
}