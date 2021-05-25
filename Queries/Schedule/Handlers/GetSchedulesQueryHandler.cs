using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeAssistantAPI.Interfaces;
using MediatR;

namespace HomeAssistantAPI.Queries.Schedule.Handlers
{
    public class GetSchedulesQueryHandler : Handler,
        IRequestHandler<GetSchedulesQuery, IEnumerable<Models.Schedule>>
    {
        private readonly IScheduleRepository _repository;

        public GetSchedulesQueryHandler(Identity identity, IScheduleRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.Schedule>> Handle(GetSchedulesQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetSchedulesAsync(Identity.Email);
        }
    }
}