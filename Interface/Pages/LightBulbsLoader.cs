﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IList<LightColor> _lightColors = new List<LightColor>();
        private IList<LightBulb> _lightBulbs;
        private string _newLightBulbName;

        private async Task GetLightBulbs()
        {
            var responseLightBulbs = await Http.GetFromJsonAsync<IList<LightBulb>>(
            $"houses/{_houseId}/rooms/{_roomId}/light_bulbs");
            if (responseLightBulbs != null)
                _lightBulbs = new List<LightBulb>(responseLightBulbs);
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
                    await PatchDevice(patchList, LightBulbsPath, lightBulb.Id);
                }

                if (lightBulb.Intensity == null)
                {
                    lightBulb.Intensity = 0;
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity.Value));
                    await PatchDevice(patchList, LightBulbsPath, lightBulb.Id);
                }
            }
        }

        private async Task AddLightBulb()
        {
            if (string.IsNullOrWhiteSpace(_newLightBulbName)) return;

            var response = await Http.PostAsJsonAsync(
            $"houses/{_houseId}/rooms/{_roomId}/{LightBulbsPath}",
            new LightBulb
            {
                Name = _newLightBulbName
            });
            if (response.IsSuccessStatusCode)
            {
                var newLightBulb = await response.Content.ReadFromJsonAsync<LightBulb>();
                _lightBulbs.Add(newLightBulb);
                _lightColors.Add(new LightColor());

                _newLightBulbName = string.Empty;
                StateHasChanged();
            }
            else
            {
                await JsRuntime.InvokeVoidAsync("alert", "Maximum number of light bulbs reached!");
            }
        }

        private async Task DeleteLightBulb(Guid id)
        {
            await Http.DeleteAsync($"houses/{_houseId}/rooms/{_roomId}/{LightBulbsPath}/{id}");
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
            await PatchDevice(patchList, LightBulbsPath, lightBulb.Id);
        }

        private async Task SetWhiteColorAndPatchLightBulb(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            var index = _lightBulbs.IndexOf(lightBulb);
            _lightColors[index] = new LightColor("#FFFFFF");
            lightBulb.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbColorPatch(lightBulb.Color.Value));
            await PatchDevice(patchList, LightBulbsPath, lightBulb.Id);
        }

        private async Task SetOffColorAndPatchLightBulb(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            var index = _lightBulbs.IndexOf(lightBulb);
            _lightColors[index] = new LightColor("#000000");
            lightBulb.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbColorPatch(lightBulb.Color.Value));
            await PatchDevice(patchList, LightBulbsPath, lightBulb.Id);
        }

        private async Task PatchLightBulbIntensity(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity ?? 0));
            await PatchDevice(patchList, LightBulbsPath, lightBulb.Id);
        }

        private async Task SetOffIntensityAndPatchLightBulb(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            lightBulb.Intensity = byte.MinValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity.Value));
            await PatchDevice(patchList, LightBulbsPath, lightBulb.Id);
        }

        private async Task SetMaxIntensityAndPatchLightBulb(Guid id)
        {
            var lightBulb = _lightBulbs.First(l => l.Id == id);
            lightBulb.Intensity = byte.MaxValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity.Value));
            await PatchDevice(patchList, LightBulbsPath, lightBulb.Id);
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