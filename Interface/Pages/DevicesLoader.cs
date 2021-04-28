using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Interface.Pages
{
    public partial class Devices
    {
        private static Guid _houseId;
        private static Guid _roomId;
        private const string LightBulbsPath = "light_bulbs";
        private const string ThermostatsPath = "thermostats";
        private const string DoorsPath = "doors";

        private async Task GetDevices()
        {
            await GetLightBulbs();
            await GetDoors();
            await GetThermostats();
        }

        private async Task PatchDevice(IList<Dictionary<string, string>> patchList,
            string path, Guid id)
        {
            string serializedContent = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedContent,
                Encoding.UTF8, "application/json");
            await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/{path}/{id}",
                patchBody);
        }
    }
}