﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Interface.Scripts;
using Microsoft.JSInterop;
using Shared.Models;
using Shared.Responses;

namespace Interface.Pages
{
    public partial class ScheduleEditor
    {
        private Guid _newCommandThermostatId;
        private IList<ThermostatCommandResponse> _thermostatCommands;
        private IList<Thermostat> _thermostats = new List<Thermostat>();
        private bool _addThermostatCollapsed = true;

        private async Task GetThermostats(Guid roomId)
        {
            _roomId = roomId;
            _thermostats = await _http.GetFromJsonAsync<IList<Thermostat>>(
                $"houses/{_houseId}/rooms/{_roomId}/{Paths.ThermostatsPath}");
        }

        private void SetNewCommandThermostatId(Guid thermostatId)
        {
            _newCommandThermostatId = thermostatId;
        }

        private async Task GetThermostatCommands()
        {
            var responseThermostatCommands = await _http.GetFromJsonAsync<IList<ThermostatCommandResponse>>(
                $"schedules/{_scheduleId}/{Paths.ThermostatCommandsPath}");
            if (responseThermostatCommands != null)
            {
                _thermostatCommands = new List<ThermostatCommandResponse>(responseThermostatCommands);
                foreach (var thermostatCommand in _thermostatCommands)
                {
                    thermostatCommand.Temperature -= (decimal) 7.0;
                }
            }
        }

        private async Task AddThermostatCommand()
        {
            if (_newCommandThermostatId == Guid.Empty) return;

            var response = await _http.PostAsJsonAsync(
                $"schedules/{_scheduleId}/{Paths.ThermostatCommandsPath}",
                new ThermostatCommand
                {
                    ThermostatId = _newCommandThermostatId,
                    Temperature = new decimal(7.0)
                });
            if (response.IsSuccessStatusCode)
            {
                var newThermostatCommand = await response.Content.ReadFromJsonAsync<ThermostatCommandResponse>();
                if (newThermostatCommand != null)
                {
                    newThermostatCommand = await _http.GetFromJsonAsync<ThermostatCommandResponse>(
                    $"schedules/{_scheduleId}/{Paths.ThermostatCommandsPath}/{newThermostatCommand.Id}");
                    if (newThermostatCommand != null)
                    {
                        newThermostatCommand.Temperature -= (decimal) 7.0;
                        _thermostatCommands.Add(newThermostatCommand);
                    }
                }

                _newCommandThermostatId = Guid.Empty;
                _addThermostatCollapsed = !_addThermostatCollapsed;
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

        private async Task DeleteThermostatCommand(Guid id)
        {
            await _http.DeleteAsync($"schedules/{_scheduleId}/{Paths.ThermostatCommandsPath}/{id}");
            _thermostatCommands.Remove(
                _thermostatCommands.SingleOrDefault(thermostatCommand => thermostatCommand.Id == id));
            StateHasChanged();
        }

        private async Task PatchThermostatCommandTemperature(Guid id)
        {
            var thermostatCommand = _thermostatCommands.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatCommandTemperaturePatch(thermostatCommand.Temperature));
            await PatchCommand(patchList, Paths.ThermostatCommandsPath, thermostatCommand.Id);
        }

        private async Task SetMinTemperatureAndPatchThermostatCommand(Guid id)
        {
            var thermostatCommand = _thermostatCommands.First(l => l.Id == id);
            thermostatCommand.Temperature = 0;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatCommandTemperaturePatch(thermostatCommand.Temperature));
            await PatchCommand(patchList, Paths.ThermostatCommandsPath, thermostatCommand.Id);
        }

        private async Task SetMaxTemperatureAndPatchThermostatCommand(Guid id)
        {
            var thermostatCommand = _thermostatCommands.First(l => l.Id == id);
            thermostatCommand.Temperature = 23;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatCommandTemperaturePatch(thermostatCommand.Temperature));
            await PatchCommand(patchList, Paths.ThermostatCommandsPath, thermostatCommand.Id);
        }

        private static Dictionary<string, string> GenerateThermostatCommandTemperaturePatch(decimal temperature)
        {
            return new()
            {
                {
                    "op", "replace"
                },
                {
                    "path", "temperature"
                },
                {
                    "value", (temperature + 7).ToString(CultureInfo.InvariantCulture)
                }
            };
        }
    }
}