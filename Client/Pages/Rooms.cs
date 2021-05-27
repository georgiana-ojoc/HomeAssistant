using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models;
using Client.Utility;
using Microsoft.JSInterop;

namespace Client.Pages
{
    public partial class Rooms
    {
        private Guid _houseId;
        private House _currentHouse = new();
        private bool _addRoomCollapsed = true;
        private Room _newRoom = new();
        private IList<Room> _rooms;

        protected override async Task OnInitializedAsync()
        {
            _houseId = await _idService.GetHouseId();
            _currentHouse = await _http.GetFromJsonAsync<House>($"{Path.Houses}/{_houseId}");
            _rooms = await _http.GetFromJsonAsync<IList<Room>>($"{Path.Houses}/{_houseId}/{Path.Rooms}");
        }

        private async Task AddRoom()
        {
            var response = await _http.PostAsJsonAsync($"{Path.Houses}/{_houseId}/{Path.Rooms}", _newRoom);
            if (response.IsSuccessStatusCode)
            {
                var newRoom = await response.Content.ReadFromJsonAsync<Room>();
                _rooms.Add(newRoom);
                _addRoomCollapsed = !_addRoomCollapsed;
                _newRoom = new Room();
                StateHasChanged();
            }
            else
            {
                if (response.StatusCode is HttpStatusCode.PaymentRequired or HttpStatusCode.Conflict)
                {
                    await _jsRuntime.InvokeVoidAsync("alert", await response.Content.ReadAsStringAsync());
                }
            }
        }

        private async Task DeleteRoom(Guid id)
        {
            await _http.DeleteAsync($"{Path.Houses}/{_houseId}/{Path.Rooms}/{id}");
            _rooms.Remove(_rooms.SingleOrDefault(room => room.Id == id));
            StateHasChanged();
        }

        private async Task SetRoomId(Guid roomId)
        {
            await _idService.SetRoomId(roomId);
            _navManager.NavigateTo("devices");
        }
    }
}