using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Rooms
    {
        private Guid _houseId;
        private Room _newRoom = new();
        private IList<Room> _rooms;
        private bool _addRoomCollapsed = true;
        private House _currentHouse = new();

        private async Task AddRoom()
        {
            var response = await _http.PostAsJsonAsync($"houses/{_houseId}/rooms", _newRoom);
            if (response.IsSuccessStatusCode)
            {
                var newRoom = await response.Content.ReadFromJsonAsync<Room>();
                _rooms.Add(newRoom);

                _newRoom = new Room();
                _addRoomCollapsed = !_addRoomCollapsed;
                StateHasChanged();
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.PaymentRequired)
                {
                    await _jsRuntime.InvokeVoidAsync("alert", await response.Content.ReadAsStringAsync());
                }

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    await _jsRuntime.InvokeVoidAsync("alert", await response.Content.ReadAsStringAsync());
                }
            }
        }

        private async Task DeleteRoom(Guid id)
        {
            await _http.DeleteAsync($"houses/{_houseId}/rooms/{id}");
            _rooms.Remove(_rooms.SingleOrDefault(room => room.Id == id));
            StateHasChanged();
        }

        private async Task SetRoomId(Guid roomId)
        {
            await _idService.SetRoomId(roomId);
            _navManager.NavigateTo("Devices");
        }
    }
}