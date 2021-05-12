using System.Threading;
using System.Threading.Tasks;
using API.Commands.Schedule;
using API.Interfaces;
using MediatR;

namespace API.Commands.Schedule.Handlers
{
    public class PartialUpdateScheduleCommandHandler : Handler,
        IRequestHandler<PartialUpdateScheduleCommand, Shared.Models.Schedule>
    {
        private readonly IScheduleRepository _repository;

        public PartialUpdateScheduleCommandHandler(Identity identity, IScheduleRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Schedule> Handle(PartialUpdateScheduleCommand request,
            CancellationToken cancellationToken)
        {
            return await _repository.PartialUpdateScheduleAsync(Identity.Email, request.Id, request.Patch);
        }
    }
}