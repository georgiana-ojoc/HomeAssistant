using System;
using System.Collections.Generic;
using System.Globalization;
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
        private bool _addThermostatCollapsed = true;
        private Guid _newCommandThermostatId;
        private IList<Thermostat> _thermostats = new List<Thermostat>();
        private IList<ThermostatCommandResponse> _thermostatCommands;

        private async Task GetThermostats(Guid roomId)
        {
            _roomId = roomId;
            _thermostats = await _http.GetFromJsonAsync<IList<Thermostat>>($"{Path.Houses}/{_houseId}/" +
                                                                           $"{Path.Rooms}/{_roomId}/{Path.Thermostats}");
        }

        private void SetNewCommandThermostatId(Guid thermostatId)
        {
            _newCommandThermostatId = thermostatId;
        }

        private async Task GetThermostatCommands()
        {
            var responseThermostatCommands = await _http.GetFromJsonAsync<IList<ThermostatCommandResponse>>(
                $"{Path.Schedules}/{_scheduleId}/{Path.ThermostatCommands}");
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
            if (_newCommandThermostatId == Guid.Empty)
            {
                return;
            }

            var response = await _http.PostAsJsonAsync($"{Path.Schedules}/{_scheduleId}/" +
                                                       $"{Path.ThermostatCommands}",
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
                        $"{Path.Schedules}/{_scheduleId}/{Path.ThermostatCommands}/{newThermostatCommand.Id}");
                    if (newThermostatCommand != null)
                    {
                        newThermostatCommand.Temperature -= (decimal) 7.0;
                        _thermostatCommands.Add(newThermostatCommand);
                    }
                }

                _addThermostatCollapsed = !_addThermostatCollapsed;
                _newCommandThermostatId = Guid.Empty;
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

        private async Task DeleteThermostatCommand(Guid id)
        {
            await _http.DeleteAsync($"{Path.Schedules}/{_scheduleId}/{Path.ThermostatCommands}/{id}");
            _thermostatCommands.Remove(_thermostatCommands.SingleOrDefault(thermostatCommand =>
                thermostatCommand.Id == id));
            StateHasChanged();
        }

        private async Task PatchThermostatCommandTemperature(Guid id)
        {
            var thermostatCommand = _thermostatCommands.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatCommandTemperaturePatch(thermostatCommand.Temperature));
            await PatchCommand(patchList, Path.ThermostatCommands, thermostatCommand.Id);
        }

        private async Task SetMinTemperatureAndPatchThermostatCommand(Guid id)
        {
            var thermostatCommand = _thermostatCommands.First(l => l.Id == id);
            thermostatCommand.Temperature = 0;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatCommandTemperaturePatch(thermostatCommand.Temperature));
            await PatchCommand(patchList, Path.ThermostatCommands, thermostatCommand.Id);
        }

        private async Task SetMaxTemperatureAndPatchThermostatCommand(Guid id)
        {
            var thermostatCommand = _thermostatCommands.First(l => l.Id == id);
            thermostatCommand.Temperature = 23;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatCommandTemperaturePatch(thermostatCommand.Temperature));
            await PatchCommand(patchList, Path.ThermostatCommands, thermostatCommand.Id);
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