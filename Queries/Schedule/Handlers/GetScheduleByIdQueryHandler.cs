using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.Schedule.Handlers
{
    public class GetScheduleByIdQueryHandler : Handler, IRequestHandler<GetScheduleByIdQuery, Models.Schedule>
    {
        private readonly IScheduleRepository _repository;

        public GetScheduleByIdQueryHandler(Identity identity, IScheduleRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Models.Schedule> Handle(GetScheduleByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetScheduleByIdAsync(Identity.Email, request.Id);
        }
    }
}