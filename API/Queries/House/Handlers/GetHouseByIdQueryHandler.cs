using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.House.Handlers
{
    public class GetHouseByIdQueryHandler : Handler, IRequestHandler<GetHouseByIdQuery, Shared.Models.House>
    {
        private readonly IHouseRepository _repository;

        public GetHouseByIdQueryHandler(Identity identity, IHouseRepository repository) : base(identity)
        {
            _repository = repository;
        }

        public async Task<Shared.Models.House> Handle(GetHouseByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetHouseByIdAsync(Identity.Email, request.Id);
        }
    }
}