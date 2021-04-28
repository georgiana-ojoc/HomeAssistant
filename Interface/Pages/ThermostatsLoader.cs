﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Devices
    {
        private IList<Thermostat> _thermostats;
        private string _newThermostatName;

        private async Task GetThermostats()
        {
            IList<Thermostat> responseThermostats = await Http.GetFromJsonAsync<IList<Thermostat>>(
                $"houses/{_houseId}/rooms/{_roomId}/{ThermostatsPath}");
            if (responseThermostats != null)
            {
                _thermostats = new List<Thermostat>(responseThermostats);
            }

            foreach (var thermostat in _thermostats)
            {
                if (thermostat.Temperature == null)
                {
                    thermostat.Temperature = 7;
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature.Value));
                    string serializedContent = JsonConvert.SerializeObject(patchList);
                    HttpContent patchBody = new StringContent(serializedContent,
                        Encoding.UTF8, "application/json");
                    await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/{ThermostatsPath}/{thermostat.Id}",
                        patchBody);
                }
            }
        }

        private async Task AddThermostat()
        {
            if (string.IsNullOrWhiteSpace(_newThermostatName))
            {
                return;
            }

            HttpResponseMessage response = await Http.PostAsJsonAsync(
                $"houses/{_houseId}/rooms/{_roomId}/{ThermostatsPath}",
                new Thermostat()
                {
                    Name = _newThermostatName
                });
            Thermostat newThermostat = await response.Content.ReadFromJsonAsync<Thermostat>();
            _thermostats.Add(newThermostat);

            _newThermostatName = string.Empty;
            StateHasChanged();
        }

        private async Task DeleteThermostat(Guid id)
        {
            await Http.DeleteAsync($"houses/{_houseId}/rooms/{_roomId}/{ThermostatsPath}/{id}");
            _thermostats.Remove(_thermostats.SingleOrDefault(thermostat => thermostat.Id == id));
            StateHasChanged();
        }

        private async Task PatchThermostatTemperature(Guid id)
        {
            Thermostat thermostat = _thermostats.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, ThermostatsPath, thermostat.Id);
        }

        private async Task SetMinTemperatureAndPatchThermostat(Guid id)
        {
            Thermostat thermostat = _thermostats.First(l => l.Id == id);
            thermostat.Temperature = 7;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, ThermostatsPath, thermostat.Id);
        }

        private async Task SetMaxTemperatureAndPatchThermostat(Guid id)
        {
            Thermostat thermostat = _thermostats.First(l => l.Id == id);
            thermostat.Temperature = 30;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, ThermostatsPath, thermostat.Id);
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
                    "value", temperature.ToString(CultureInfo.InvariantCulture)
                }
            };
        }
    }
}