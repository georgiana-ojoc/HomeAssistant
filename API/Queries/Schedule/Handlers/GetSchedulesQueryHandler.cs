using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Schedule.Handlers
{
    public class GetSchedulesQueryHandler:Handler,IRequestHandler<GetSchedulesQuery,IEnumerable<Shared.Models.Schedule>>
    {
        private readonly IScheduleRepository _repository;

        public GetSchedulesQueryHandler(Identity identity, IScheduleRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Shared.Models.Schedule>> Handle(GetSchedulesQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetSchedulesAsync(Identity.Email);
        }
    }
}