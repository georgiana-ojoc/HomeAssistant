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
        private readonly IList<LightColor> _lightColors = new List<LightColor>();
        private bool _addLightBulbCollapsed = true;
        private LightBulb _newLightBulb = new();
        private IList<LightBulb> _lightBulbs;

        private async Task GetLightBulbs()
        {
            var response = await _http.GetFromJsonAsync<IList<LightBulb>>(
                $"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}/{Path.LightBulbs}");
            if (response != null)
            {
                _lightBulbs = new List<LightBulb>(response);
            }

            foreach (var lightBulb in _lightBulbs)
            {
                if (lightBulb.Color != null)
                {
                    _lightColors.Add(new LightColor(lightBulb.Color.Value));
                }
                else
                {
                    _lightColors.Add(new LightColor());
                    lightBulb.Color = new LightColor().GetIntColor();
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateLightBulbColorPatch(lightBulb.Color.Value));
                    await PatchDevice(patchList, Path.LightBulbs, lightBulb.Id);
                }

                if (lightBulb.Intensity == null)
                {
                    lightBulb.Intensity = 0;
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity.Value));
                    await PatchDevice(patchList, Path.LightBulbs, lightBulb.Id);
                }
            }
        }

        private async Task AddLightBulb()
        {
            var response = await _http.PostAsJsonAsync(
                $"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}/{Path.LightBulbs}", _newLightBulb);
            if (response.IsSuccessStatusCode)
            {
                var newLightBulb = await response.Content.ReadFromJsonAsync<LightBulb>();
                _lightBulbs.Add(newLightBulb);
                _lightColors.Add(new LightColor());
                _addLightBulbCollapsed = !_addLightBulbCollapsed;
                _newLightBulb = new LightBulb();
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

        private async Task DeleteLightBulb(Guid id)
        {
            await _http.DeleteAsync($"{Path.Houses}/{_houseId}/{Path.Rooms}/{_roomId}/" +
                                    $"{Path.LightBulbs}/{id}");
            _lightBulbs.Remove(_lightBulbs.SingleOrDefault(lightBulb => lightBulb.Id == id));
            StateHasChanged();
        }

        private async Task PatchLightBulbColor(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            var index = _lightBulbs.IndexOf(lightBulb);
            lightBulb.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbColorPatch(lightBulb.Color.Value));
            await PatchDevice(patchList, Path.LightBulbs, lightBulb.Id);
        }

        private async Task SetOnColorAndPatchLightBulb(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            var index = _lightBulbs.IndexOf(lightBulb);
            _lightColors[index] = new LightColor("#FFFFFF");
            lightBulb.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbColorPatch(lightBulb.Color.Value));
            await PatchDevice(patchList, Path.LightBulbs, lightBulb.Id);
        }

        private async Task SetOffColorAndPatchLightBulb(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            var index = _lightBulbs.IndexOf(lightBulb);
            _lightColors[index] = new LightColor("#000000");
            lightBulb.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbColorPatch(lightBulb.Color.Value));
            await PatchDevice(patchList, Path.LightBulbs, lightBulb.Id);
        }

        private async Task PatchLightBulbIntensity(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity ?? 0));
            await PatchDevice(patchList, Path.LightBulbs, lightBulb.Id);
        }

        private async Task SetMinIntensityAndPatchLightBulb(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            lightBulb.Intensity = byte.MinValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity.Value));
            await PatchDevice(patchList, Path.LightBulbs, lightBulb.Id);
        }

        private async Task SetMaxIntensityAndPatchLightBulb(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            lightBulb.Intensity = byte.MaxValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity.Value));
            await PatchDevice(patchList, Path.LightBulbs, lightBulb.Id);
        }

        private static Dictionary<string, string> GenerateLightBulbColorPatch(int color)
        {
            return new()
            {
                {
                    "op", "replace"
                },
                {
                    "path", "color"
                },
                {
                    "value", color.ToString()
                }
            };
        }

        private static Dictionary<string, string> GenerateLightBulbIntensityPatch(byte intensity)
        {
            return new()
            {
                {
                    "op", "replace"
                },
                {
                    "path", "intensity"
                },
                {
                    "value", intensity.ToString()
                }
            };
        }
    }
}