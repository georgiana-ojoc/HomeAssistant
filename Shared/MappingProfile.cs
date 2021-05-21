using AutoMapper;
using Shared.Models;
using Shared.Requests;

namespace Shared
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserLimit, UserLimitRequest>();
            CreateMap<UserLimitRequest, UserLimit>();
            
            CreateMap<House, HouseRequest>();
            CreateMap<HouseRequest, House>();

            CreateMap<Room, RoomRequest>();
            CreateMap<RoomRequest, Room>();

            CreateMap<LightBulb, LightBulbRequest>();
            CreateMap<LightBulbRequest, LightBulb>();

            CreateMap<Door, DoorRequest>();
            CreateMap<DoorRequest, Door>();

            CreateMap<Thermostat, ThermostatRequest>();
            CreateMap<ThermostatRequest, Thermostat>();

            CreateMap<Schedule, ScheduleRequest>();
            CreateMap<ScheduleRequest, Schedule>();

            CreateMap<LightBulbCommand, LightBulbCommandRequest>();
            CreateMap<LightBulbCommandRequest, LightBulbCommand>();

            CreateMap<DoorCommand, DoorCommandRequest>();
            CreateMap<DoorCommandRequest, DoorCommand>();

            CreateMap<ThermostatCommand, ThermostatCommandRequest>();
            CreateMap<ThermostatCommandRequest, ThermostatCommand>();
        }
    }
}