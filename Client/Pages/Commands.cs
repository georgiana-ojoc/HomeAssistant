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
    public partial class Commands
    {
        private Guid _houseId = Guid.Empty;
        private Guid _roomId = Guid.Empty;
        private Guid _scheduleId;
        private Schedule _schedule = new();
        private IList<House> _houses = new List<House>();
        private IList<Room> _rooms = new List<Room>();

        protected override async Task OnInitializedAsync()
        {
            _scheduleId = await _idService.GetScheduleId();
            await GetSchedule();
            await GetHouses();
            await GetCommands();
        }

        private async Task GetSchedule()
        {
            _schedule = await _http.GetFromJsonAsync<Schedule>($"{Path.Schedules}/{_scheduleId}");
        }

        private async Task GetHouses()
        {
            _houseId = Guid.Empty;
            _houses = await _http.GetFromJsonAsync<IList<House>>($"{Path.Houses}");
        }

        private async Task GetRooms(Guid houseId)
        {
            _houseId = houseId;
            _roomId = Guid.Empty;
            _rooms = await _http.GetFromJsonAsync<IList<Room>>($"{Path.Houses}/{_houseId}/{Path.Rooms}");
        }

        private async Task GetCommands()
        {
            await GetLightBulbCommands();
            await GetDoorCommands();
            await GetThermostatCommands();
        }

        private async Task PatchCommand(IList<Dictionary<string, string>> patchList, string path, Guid id)
        {
            var serializeObject = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializeObject, Encoding.UTF8, "application/json");
            await _http.PatchAsync($"{Path.Schedules}/{_scheduleId}/{path}/{id}", patchBody);
        }
    }
}