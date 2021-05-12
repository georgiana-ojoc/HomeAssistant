using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace API.Queries.Schedule.Handlers
{
    public class GetScheduleByIdQueryHandler:Handler,IRequestHandler<GetScheduleByIdQuery,Shared.Models.Schedule>
    {
        private readonly IScheduleRepository _repository;

        public GetScheduleByIdQueryHandler(Identity identity, IScheduleRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.Schedule> Handle(GetScheduleByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _repository.GetScheduleByIdAsync(Identity.Email, request.Id);
        }
    }
}