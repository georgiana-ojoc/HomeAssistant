using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Devices
    {
        private IList<Door> _doors;
        private string _newDoorName;

        private async Task GetDoors()
        {
            var responseDoors =
                await Http.GetFromJsonAsync<IList<Door>>($"houses/{_houseId}/rooms/{_roomId}/{DoorsPath}");
            if (responseDoors != null) _doors = new List<Door>(responseDoors);

            foreach (var door in _doors)
                if (door.Locked == null)
                {
                    door.Locked = false;
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateDoorLockedPatch(door.Locked.Value));
                    await PatchDevice(patchList, DoorsPath, door.Id);
                }
        }

        private async Task AddDoor()
        {
            if (string.IsNullOrWhiteSpace(_newDoorName)) return;

            var response = await Http.PostAsJsonAsync($"houses/{_houseId}/rooms/{_roomId}/{DoorsPath}",
                new Door
                {
                    Name = _newDoorName
                });
            if (response.IsSuccessStatusCode)
            {
                var newDoor = await response.Content.ReadFromJsonAsync<Door>();
                _doors.Add(newDoor);

                _newDoorName = string.Empty;
                StateHasChanged();
            }
            else
            {
                await JsRuntime.InvokeVoidAsync("alert", "Maximum number of doors reached!");
            }
        }

        private async Task DeleteDoor(Guid id)
        {
            await Http.DeleteAsync($"houses/{_houseId}/rooms/{_roomId}/doors/{id}");
            _doors.Remove(_doors.SingleOrDefault(door => door.Id == id));
            StateHasChanged();
        }

        private async Task SetTrueLockedAndPatchDoor(Guid id)
        {
            var door = _doors.First(l => l.Id == id);
            door.Locked = true;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateDoorLockedPatch(door.Locked.Value));
            await PatchDevice(patchList, DoorsPath, door.Id);
        }

        private async Task SetFalseLockedAndPatchDoor(Guid id)
        {
            var door = _doors.First(l => l.Id == id);
            door.Locked = false;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateDoorLockedPatch(door.Locked.Value));
            await PatchDevice(patchList, DoorsPath, door.Id);
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