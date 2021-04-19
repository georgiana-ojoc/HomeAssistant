using AutoMapper;
using Shared.Models;
using Shared.Models.Patch;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            // Add as many of these lines as you need to map your objects
            CreateMap<House, HousePatch>();
            CreateMap<HousePatch, House>();
            
            CreateMap<Room, RoomPatch>();
            CreateMap<RoomPatch, Room>();
            
            CreateMap<Door, DoorPatch>();
            CreateMap<DoorPatch, Door>();
            
            CreateMap<LightBulb, LightBulbPatch>();
            CreateMap<LightBulbPatch, LightBulb>();
            
            CreateMap<Thermostat, ThermostatPatch>();
            CreateMap<ThermostatPatch, Thermostat>();
        }
    }
}