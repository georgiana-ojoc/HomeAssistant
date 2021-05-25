﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Interface.Scripts;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Devices
    {
        private Thermostat _newThermostat = new();
        private IList<Thermostat> _thermostats;
        private bool _addThermostatCollapsed = true;

        private async Task GetThermostats()
        {
            var responseThermostats = await _http.GetFromJsonAsync<IList<Thermostat>>(
                $"houses/{_houseId}/rooms/{_roomId}/{Paths.ThermostatsPath}");
            if (responseThermostats != null) _thermostats = new List<Thermostat>(responseThermostats);

            foreach (var thermostat in _thermostats)
                if (thermostat.Temperature == null)
                {
                    thermostat.Temperature = 0;
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature.Value));
                    var serializedContent = JsonConvert.SerializeObject(patchList);
                    HttpContent patchBody = new StringContent(serializedContent,
                        Encoding.UTF8,
                        "application/json");
                    await _http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/{Paths.ThermostatsPath}/{thermostat.Id}",
                        patchBody);
                }
                else
                {
                    thermostat.Temperature -= 7;
                }
        }

        private async Task AddThermostat()
        {
            var response = await _http.PostAsJsonAsync(
                $"houses/{_houseId}/rooms/{_roomId}/{Paths.ThermostatsPath}",_newThermostat);
            if (response.IsSuccessStatusCode)
            {
                var newThermostat = await response.Content.ReadFromJsonAsync<Thermostat>();
                if (newThermostat != null)
                {
                    newThermostat.Temperature -= 7;
                    _thermostats.Add(newThermostat);
                }

                _newThermostat = new Thermostat();
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

        private async Task DeleteThermostat(Guid id)
        {
            await _http.DeleteAsync($"houses/{_houseId}/rooms/{_roomId}/{Paths.ThermostatsPath}/{id}");
            _thermostats.Remove(_thermostats.SingleOrDefault(thermostat => thermostat.Id == id));
            StateHasChanged();
        }

        private async Task PatchThermostatTemperature(Guid id)
        {
            var thermostat = _thermostats.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, Paths.ThermostatsPath, thermostat.Id);
        }

        private async Task SetMinTemperatureAndPatchThermostat(Guid id)
        {
            var thermostat = _thermostats.First(l => l.Id == id);
            thermostat.Temperature = 0;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, Paths.ThermostatsPath, thermostat.Id);
        }

        private async Task SetMaxTemperatureAndPatchThermostat(Guid id)
        {
            var thermostat = _thermostats.First(l => l.Id == id);
            thermostat.Temperature = 23;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, Paths.ThermostatsPath, thermostat.Id);
        }

        private static Dictionary<string, string> GenerateThermostatTemperaturePatch(decimal temperature)
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