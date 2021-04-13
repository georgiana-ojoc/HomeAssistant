using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using MediatR;

namespace API.Queries.Room.Handlers
{
    public class RoomsQueryHandler: IRequestHandler<RoomsQuery,IEnumerable<Models.Room>>
    {
        private readonly IRoomRepository _repository;

        public RoomsQueryHandler(IRoomRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Models.Room>> Handle(RoomsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Models.Room> rooms = await _repository.GetRoomsAsync(request.UserId, request.HouseId);
            return rooms;
        }
    }
}