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
    public partial class ScheduleEditor
    {
        private static Guid _scheduleId;
        private Guid _houseId = Guid.Empty;
        private Guid _roomId = Guid.Empty;
        
        private IList<House> _houses = new List<House>();
        private IList<Room> _rooms = new List<Room>();

        private async Task GetHouses()
        {
            _houseId = Guid.Empty;
            _houses = await _http.GetFromJsonAsync<IList<House>>($"houses");
        }

        private async Task GetRooms(Guid houseId)
        {
            _roomId = Guid.Empty;
            _houseId = houseId;
            _rooms = await _http.GetFromJsonAsync<IList<Room>>($"houses/{_houseId}/rooms");
        }

        private async Task GetCommands()
        {
            await GetLightBulbCommands();
            await GetDoorCommands();
            await GetThermostatCommands();
        }

        private async Task PatchCommand(IList<Dictionary<string, string>> patchList,
            string path, Guid id)
        {
            var serializedContent = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedContent,
            Encoding.UTF8,
            "application/json");
            await _http.PatchAsync($"schedules/{_scheduleId}/{path}/{id}",
            patchBody);
        }
    }
}