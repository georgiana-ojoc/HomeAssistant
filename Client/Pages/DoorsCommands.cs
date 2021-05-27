using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models;
using Client.Responses;
using Client.Utility;
using Microsoft.JSInterop;

namespace Client.Pages
{
    public partial class Commands
    {
        private bool _addDoorCollapsed = true;
        private Guid _newCommandDoorId;
        private IList<Door> _doors = new List<Door>();
        private IList<DoorCommandResponse> _doorCommands;

        private async Task GetDoors(Guid roomId)
        {
            _roomId = roomId;
            _doors = await _http.GetFromJsonAsync<IList<Door>>($"{Path.Houses}/{_houseId}/{Path.Rooms}/" +
                                                               $"{_roomId}/{Path.Doors}");
        }

        private void SetNewCommandDoorId(Guid doorId)
        {
            _newCommandDoorId = doorId;
        }

        private async Task GetDoorCommands()
        {
            var response = await _http.GetFromJsonAsync<IList<DoorCommandResponse>>(
                $"{Path.Schedules}/{_scheduleId}/{Path.DoorCommands}");
            if (response != null)
            {
                _doorCommands = new List<DoorCommandResponse>(response);
            }
        }

        private async Task AddDoorCommand()
        {
            if (_newCommandDoorId == Guid.Empty)
            {
                return;
            }

            var response = await _http.PostAsJsonAsync($"{Path.Schedules}/{_scheduleId}/{Path.DoorCommands}",
                new DoorCommand
                {
                    DoorId = _newCommandDoorId,
                    Locked = false
                });
            if (response.IsSuccessStatusCode)
            {
                var newDoorCommand = await response.Content.ReadFromJsonAsync<DoorCommandResponse>();
                if (newDoorCommand != null)
                {
                    newDoorCommand = await _http.GetFromJsonAsync<DoorCommandResponse>(
                        $"{Path.Schedules}/{_scheduleId}/{Path.DoorCommands}/{newDoorCommand.Id}");
                    _doorCommands.Add(newDoorCommand);
                }

                _addDoorCollapsed = !_addDoorCollapsed;
                _newCommandDoorId = Guid.Empty;
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

        private async Task DeleteDoorCommand(Guid id)
        {
            await _http.DeleteAsync($"{Path.Schedules}/{_scheduleId}/{Path.DoorCommands}/{id}");
            _doorCommands.Remove(_doorCommands.SingleOrDefault(doorCommand => doorCommand.Id == id));
            StateHasChanged();
        }

        private async Task SetTrueLockedAndPatchDoorCommand(Guid id)
        {
            var doorCommand = _doorCommands.First(l => l.Id == id);
            doorCommand.Locked = true;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateDoorCommandLockedPatch(doorCommand.Locked));
            await PatchCommand(patchList, Path.DoorCommands, doorCommand.Id);
        }

        private async Task SetFalseLockedAndPatchDoorCommand(Guid id)
        {
            var doorCommand = _doorCommands.First(l => l.Id == id);
            doorCommand.Locked = false;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateDoorCommandLockedPatch(doorCommand.Locked));
            await PatchCommand(patchList, Path.DoorCommands, doorCommand.Id);
        }

        private static Dictionary<string, string> GenerateDoorCommandLockedPatch(bool locked)
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