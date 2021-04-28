using AutoMapper;
using Shared.Models;
using Shared.Requests;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<House, HouseRequest>();
            CreateMap<HouseRequest, House>();

            CreateMap<Room, RoomRequest>();
            CreateMap<RoomRequest, Room>();

            CreateMap<Door, DoorRequest>();
            CreateMap<DoorRequest, Door>();

            CreateMap<LightBulb, LightBulbRequest>();
            CreateMap<LightBulbRequest, LightBulb>();

            CreateMap<Thermostat, ThermostatRequest>();
            CreateMap<ThermostatRequest, Thermostat>();
        }
    }
}