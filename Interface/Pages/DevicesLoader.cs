using System;
using System.Threading.Tasks;

namespace Interface.Pages
{
    public partial class Devices
    {
        private static Guid _houseId;
        private static Guid _roomId;

        private async Task GetDevices()
        {
            await GetLightBulbs();
            await GetDoors();
            await GetThermostats();
        }
    }
}