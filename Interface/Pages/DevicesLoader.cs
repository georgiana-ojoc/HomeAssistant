using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Devices
    {
        private static Guid _houseId;
        private static Guid _roomId;
        private Room _currentRoom = new();

        private async Task GetDevices()
        {
            await GetLightBulbs();
            await GetDoors();
            await GetThermostats();
        }

        private async Task GetCurrentRoom()
        {
            _currentRoom = await _http.GetFromJsonAsync<Room>($"houses/{_houseId}/rooms/{_roomId}");
        }

        private async Task PatchDevice(IList<Dictionary<string, string>> patchList,
            string path, Guid id)
        {
            var serializedContent = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedContent,
                Encoding.UTF8,
                "application/json");
            await _http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/{path}/{id}",
                patchBody);
        }
    }
}