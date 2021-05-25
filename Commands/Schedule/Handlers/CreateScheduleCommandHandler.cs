using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Commands.Schedule.Handlers
{
    public class CreateScheduleCommandHandler : Handler, IRequestHandler<CreateScheduleCommand, Models.Schedule>
    {
        private readonly IScheduleRepository _repository;

        public CreateScheduleCommandHandler(Identity identity, IScheduleRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Schedule> Handle(CreateScheduleCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateScheduleAsync(Identity.Email, new Models.Schedule
            {
                Name = request.Request.Name,
                Time = request.Request.Time,
                Days = request.Request.Days
            });
        }
    }
}