﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Interface.Scripts;
using Microsoft.JSInterop;
using Shared.Models;

namespace Interface.Pages
{
    public partial class ScheduleEditor
    {
        private IList<DoorCommand> _doorCommands;
        private Guid _newCommandDoorId;
        private IList<Door> _doors = new List<Door>();

        private async Task GetDoors(Guid roomId)
        {
            _roomId = roomId;
            _doors = await _http.GetFromJsonAsync<IList<Door>>(
                $"houses/{_houseId}/rooms/{_roomId}/{Paths.DoorsPath}");
        }

        private void SetNewCommandDoorId(Guid doorId)
        {
            _newCommandDoorId = doorId;
        }

        private async Task GetDoorCommands()
        {
            var responseDoorCommands = await _http.GetFromJsonAsync<IList<DoorCommand>>(
                $"schedules/{_scheduleId}/{Paths.DoorCommandsPath}");
            if (responseDoorCommands != null) _doorCommands = new List<DoorCommand>(responseDoorCommands);
        }

        private async Task AddDoorCommand()
        {
            if (_newCommandDoorId == Guid.Empty) return;

            var response = await _http.PostAsJsonAsync($"schedules/{_scheduleId}/{Paths.DoorCommandsPath}",
                new DoorCommand
                {
                    DoorId = _newCommandDoorId,
                    Locked = false
                });
            if (response.IsSuccessStatusCode)
            {
                var newDoorCommand = await response.Content.ReadFromJsonAsync<DoorCommand>();
                _doorCommands.Add(newDoorCommand);

                _newCommandDoorId = Guid.Empty;
                StateHasChanged();
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    await _jsRuntime.InvokeVoidAsync("alert", "Maximum number of door commands reached!");
                }
            }
        }

        private async Task DeleteDoorCommand(Guid id)
        {
            await _http.DeleteAsync($"schedules/{_scheduleId}/{Paths.DoorCommandsPath}/{id}");
            _doorCommands.Remove(_doorCommands.SingleOrDefault(doorCommand => doorCommand.Id == id));
            StateHasChanged();
        }

        private async Task SetTrueLockedAndPatchDoorCommand(Guid id)
        {
            var doorCommand = _doorCommands.First(l => l.Id == id);
            doorCommand.Locked = true;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateDoorCommandLockedPatch(doorCommand.Locked));
            await PatchCommand(patchList, Paths.DoorCommandsPath, doorCommand.Id);
        }

        private async Task SetFalseLockedAndPatchDoorCommand(Guid id)
        {
            var doorCommand = _doorCommands.First(l => l.Id == id);
            doorCommand.Locked = false;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateDoorCommandLockedPatch(doorCommand.Locked));
            await PatchCommand(patchList, Paths.DoorCommandsPath, doorCommand.Id);
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