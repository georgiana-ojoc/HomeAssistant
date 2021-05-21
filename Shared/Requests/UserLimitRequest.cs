using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class UserLimitRequest
    {
        [Required] public int HouseLimit { get; set; }
        
        [Required] public int RoomLimit { get; set; }
        
        [Required] public int LightBulbLimit { get; set; }
        
        [Required] public int DoorLimit { get; set; }
        
        [Required] public int ThermostatLimit { get; set; }
    }
}