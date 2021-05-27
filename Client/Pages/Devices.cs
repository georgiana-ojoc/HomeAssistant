using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Client.Models;
using Client.Utility;
using Newtonsoft.Json;

namespace Client.Pages
{
    public partial class Devices
    {
        private Guid _houseId;
        private Guid _roomId;
        private Room _currentRoom = new();

        protected override async Task OnInitializedAsync()
        {
            _houseId = await _idService.GetHouseId();
            _roomId = await _idService.GetRoomId();
            await GetCurrentRoom();
            await GetDevices();
        }

        private async Task GetCurrentRoom()
        {
            _currentRoom = await _http.GetFromJsonAsync<Room>($"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}");
        }

        private async Task GetDevices()
        {
            await GetLightBulbs();
            await GetDoors();
            await GetThermostats();
        }

        private async Task PatchDevice(IList<Dictionary<string, string>> patchList, string path, Guid id)
        {
            var serializedObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            await _http.PatchAsync($"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}/{path}/{id}", patchBody);
        }
    }
}