using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Client.Models;
using Client.Utility;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Client.Pages
{
    public partial class Devices
    {
        private bool _addThermostatCollapsed = true;
        private Thermostat _newThermostat = new();
        private IList<Thermostat> _thermostats;

        private async Task GetThermostats()
        {
            var response = await _http.GetFromJsonAsync<IList<Thermostat>>($"{Path.Houses}/{_houseId}/" +
                                                                           $"{Path.Rooms}/{_roomId}/" +
                                                                           $"{Path.Thermostats}");
            if (response != null)
            {
                _thermostats = new List<Thermostat>(response);
            }

            foreach (var thermostat in _thermostats)
            {
                if (thermostat.Temperature == null)
                {
                    thermostat.Temperature = 0;
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature.Value));
                    var serializeObject = JsonConvert.SerializeObject(patchList);
                    HttpContent patchBody = new StringContent(serializeObject, Encoding.UTF8,
                        "application/json");
                    await _http.PatchAsync($"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}/" +
                                           $"{Path.Thermostats}/{thermostat.Id}", patchBody);
                }
                else
                {
                    thermostat.Temperature -= 7;
                }
            }
        }

        private async Task AddThermostat()
        {
            var response = await _http.PostAsJsonAsync($"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}/" +
                                                       $"{Path.Thermostats}", _newThermostat);
            if (response.IsSuccessStatusCode)
            {
                var newThermostat = await response.Content.ReadFromJsonAsync<Thermostat>();
                if (newThermostat != null)
                {
                    newThermostat.Temperature -= 7;
                    _thermostats.Add(newThermostat);
                }

                _addThermostatCollapsed = !_addThermostatCollapsed;
                _newThermostat = new Thermostat();
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

        private async Task DeleteThermostat(Guid id)
        {
            await _http.DeleteAsync($"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}/" +
                                    $"{Path.Thermostats}/{id}");
            _thermostats.Remove(_thermostats.SingleOrDefault(thermostat => thermostat.Id == id));
            StateHasChanged();
        }

        private async Task PatchThermostatTemperature(Guid id)
        {
            var thermostat = _thermostats.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, Path.Thermostats, thermostat.Id);
        }

        private async Task SetMinTemperatureAndPatchThermostat(Guid id)
        {
            var thermostat = _thermostats.First(l => l.Id == id);
            thermostat.Temperature = 0;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, Path.Thermostats, thermostat.Id);
        }

        private async Task SetMaxTemperatureAndPatchThermostat(Guid id)
        {
            var thermostat = _thermostats.First(l => l.Id == id);
            thermostat.Temperature = 23;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateThermostatTemperaturePatch(thermostat.Temperature ?? 0));
            await PatchDevice(patchList, Path.Thermostats, thermostat.Id);
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