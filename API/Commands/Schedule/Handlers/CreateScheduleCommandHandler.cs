using System.Threading;
using System.Threading.Tasks;
using API.Commands.Schedule;
using API.Interfaces;
using MediatR;

namespace API.Commands.Schedule.Handlers
{
    public class CreateScheduleHandler : Handler, IRequestHandler<CreateScheduleCommand, Shared.Models.Schedule>
    {
        private readonly IScheduleRepository _repository;

        public CreateScheduleHandler(Identity identity, IScheduleRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Schedule> Handle(CreateScheduleCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.CreateScheduleAsync(Identity.Email, new Shared.Models.Schedule
            {
                Name = request.Request.Name,
                Time = request.Request.Time,
                Days = request.Request.Days
            });
        }
    }
}