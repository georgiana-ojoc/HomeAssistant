using System;
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
        private string _newThermostatName;
        private IList<Thermostat> _thermostats;

        private async Task GetThermostats()
        {
            var responseThermostats = await _http.GetFromJsonAsync<IList<Thermostat>>(
                $"houses/{_houseId}/rooms/{_roomId}/{Paths.ThermostatsPath}");
            if (responseThermostats != null) _thermostats = new List<Thermostat>(responseThermostats);

            foreach (var thermostat in _thermostats)
                if (thermostat.Temperature == null)
                {
                    thermostat.Temperature = 7;
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature.Value));
                    var serializedContent = JsonConvert.SerializeObject(patchList);
                    HttpContent patchBody = new StringContent(serializedContent,
                        Encoding.UTF8,
                        "application/json");
                    await _http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/{Paths.ThermostatsPath}/{thermostat.Id}",
                        patchBody);
                }
        }

        private async Task AddThermostat()
        {
            if (string.IsNullOrWhiteSpace(_newThermostatName)) return;

            var response = await _http.PostAsJsonAsync(
                $"houses/{_houseId}/rooms/{_roomId}/{Paths.ThermostatsPath}",
                new Thermostat
                {
                    Name = _newThermostatName
                });
            if (response.IsSuccessStatusCode)
            {
                var newThermostat = await response.Content.ReadFromJsonAsync<Thermostat>();
                _thermostats.Add(newThermostat);

                _newThermostatName = string.Empty;
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
            thermostat.Temperature = 7;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, Paths.ThermostatsPath, thermostat.Id);
        }

        private async Task SetMaxTemperatureAndPatchThermostat(Guid id)
        {
            var thermostat = _thermostats.First(l => l.Id == id);
            thermostat.Temperature = 30;
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
                    "value", temperature.ToString(CultureInfo.InvariantCulture)
                }
            };
        }
    }
}