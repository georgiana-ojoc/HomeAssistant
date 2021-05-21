using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class UserLimit : BaseModel
    {
        [Required] public string Email { get; set; }
        [Required] public int HouseLimit { get; set; }
        [Required] public int RoomLimit { get; set; }
        [Required] public int LightBulbLimit { get; set; }
        [Required] public int DoorLimit { get; set; }
        [Required] public int ThermostatLimit { get; set; }
        [Required] public int ScheduleLimit { get; set; }
        [Required] public int CommandLimit { get; set; }

        public UserLimit(int houseLimit, int roomLimit, int lightBulbLimit, int doorLimit, int thermostatLimit, int scheduleLimit, int commandLimit)
        {
            HouseLimit = houseLimit;
            RoomLimit = roomLimit;
            LightBulbLimit = lightBulbLimit;
            DoorLimit = doorLimit;
            ThermostatLimit = thermostatLimit;
            ScheduleLimit = scheduleLimit;
            CommandLimit = commandLimit;
        }

        public UserLimit()
        {
        }
    }
}