using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Rooms
    {
        private IList<Room> _rooms;
        private string _newRoomName;
        private Guid _houseId;

        private async void AddRoom()
        {
            if (string.IsNullOrWhiteSpace(_newRoomName))
            {
                return;
            }
            HttpResponseMessage response = await Http.PostAsJsonAsync($"houses/{_houseId}/rooms", new Room()
            {
                Name = _newRoomName
            });
            Room newRoom = await response.Content.ReadFromJsonAsync<Room>();
            _rooms.Add(newRoom);

            _newRoomName = string.Empty;
            StateHasChanged();
        }

        private async void DeleteRoom(Guid id)
        {
            await Http.DeleteAsync($"houses/{_houseId}/rooms/{id}");
            _rooms.Remove(_rooms.SingleOrDefault(room => room.Id == id));
            StateHasChanged();
        }

        private async void SetRoomId(Guid roomId)
        {
            await IdService.SetRoomId(roomId);
            NavManager.NavigateTo("Devices");
        }
    }
}