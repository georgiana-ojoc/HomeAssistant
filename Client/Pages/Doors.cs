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
    public partial class Devices
    {
        private bool _addDoorCollapsed = true;
        private Door _newDoor = new();
        private IList<Door> _doors;

        private async Task GetDoors()
        {
            var response = await _http.GetFromJsonAsync<IList<Door>>($"{Path.Houses}/{_houseId}/" +
                                                                     $"{Path.Rooms}/{_roomId}/{Path.Doors}");
            if (response != null)
            {
                _doors = new List<Door>(response);
            }

            foreach (var door in _doors)
            {
                if (door.Locked == null)
                {
                    door.Locked = false;
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateDoorLockedPatch(door.Locked.Value));
                    await PatchDevice(patchList, Path.Doors, door.Id);
                }
            }
        }

        private async Task AddDoor()
        {
            var response = await _http.PostAsJsonAsync($"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}/" +
                                                       $"{Path.Doors}", _newDoor);
            if (response.IsSuccessStatusCode)
            {
                var newDoor = await response.Content.ReadFromJsonAsync<Door>();
                _doors.Add(newDoor);
                _addDoorCollapsed = !_addDoorCollapsed;
                _newDoor = new Door();
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

        private async Task DeleteDoor(Guid id)
        {
            await _http.DeleteAsync($"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}/doors/{id}");
            _doors.Remove(_doors.SingleOrDefault(door => door.Id == id));
            StateHasChanged();
        }

        private async Task SetTrueLockedAndPatchDoor(Guid id)
        {
            var door = _doors.First(l => l.Id == id);
            door.Locked = true;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateDoorLockedPatch(door.Locked.Value));
            await PatchDevice(patchList, Path.Doors, door.Id);
        }

        private async Task SetFalseLockedAndPatchDoor(Guid id)
        {
            var door = _doors.First(l => l.Id == id);
            door.Locked = false;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateDoorLockedPatch(door.Locked.Value));
            await PatchDevice(patchList, Path.Doors, door.Id);
        }

        private static Dictionary<string, string> GenerateDoorLockedPatch(bool locked)
        {
            return new()
            {
                {
                    "op", "replace"
                },
                {
                    "path", "locked"
                },
                {
                    "value", locked.ToString()
                }
            };
        }
    }
}