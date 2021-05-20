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
        private string _newRoomName;
        private IList<Room> _rooms;

        private async Task AddRoom()
        {
            if (string.IsNullOrWhiteSpace(_newRoomName)) return;

            var response = await _http.PostAsJsonAsync($"houses/{_houseId}/rooms",
                new Room
                {
                    Name = _newRoomName
                });
            if (response.IsSuccessStatusCode)
            {
                var newRoom = await response.Content.ReadFromJsonAsync<Room>();
                _rooms.Add(newRoom);

                _newRoomName = string.Empty;
                StateHasChanged();
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
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