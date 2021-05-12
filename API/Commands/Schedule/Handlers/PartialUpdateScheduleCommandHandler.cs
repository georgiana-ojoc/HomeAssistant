using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Commands.Schedule.Handlers
{
    public class PartialUpdateScheduleHandler : Handler,
        IRequestHandler<PartialUpdateScheduleCommand, Shared.Models.Schedule>
    {
        private readonly IScheduleRepository _repository;

        public PartialUpdateScheduleHandler(Identity identity, IScheduleRepository repository) : base(identity)
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