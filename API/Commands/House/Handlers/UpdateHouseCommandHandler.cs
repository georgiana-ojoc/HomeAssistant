using System.Threading;
using System.Threading.Tasks;
using API.Interfaces;
using AutoMapper;
using MediatR;
using Shared.Models.Patch;

namespace API.Commands.House.Handlers
{
    public class UpdateHouseCommandHandler : IRequestHandler<UpdateHouseCommand, Shared.Models.House>
    {
        private readonly IHouseRepository _repository;
        private readonly IMapper _mapper;

        public UpdateHouseCommandHandler(IHouseRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Shared.Models.House> Handle(UpdateHouseCommand request, CancellationToken cancellationToken)
        {
            Shared.Models.House house = await _repository.GetHouseByIdAsync(request.Email, request.Id);
            if (house == null)
            {
                return null;
            }

            HousePatch houseToPatch = _mapper.Map<HousePatch>(house);
            request.Patch.ApplyTo(houseToPatch);
            _mapper.Map(houseToPatch, house);

            if (!await _repository.SaveChangesAsync())
            {
                return null;
            }

            return house;
        }
    }
}