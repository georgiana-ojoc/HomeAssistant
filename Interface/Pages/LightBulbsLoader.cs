using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Interface.Scripts;
using Newtonsoft.Json;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Devices
    {
        private IList<LightBulb> _lightBulbs;
        private readonly IList<LightColor> _lightColors = new List<LightColor>();
        private string _newLightBulbName; 
        private async Task GetLightBulbs()
        {
            IList<LightBulb> responseLightBulbs = await Http.GetFromJsonAsync<IList<LightBulb>>(
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
                    string serializedContent = JsonConvert.SerializeObject(patchList);
                    HttpContent patchBody = new StringContent(serializedContent,
                    Encoding.UTF8, "application/json");
                    await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/light_bulbs/{lightBulb.Id}",
                    patchBody);
                }
                if (lightBulb.Intensity == null)
                {
                    lightBulb.Intensity = 0;
                    IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
                    patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity.Value));
                    string serializedContent = JsonConvert.SerializeObject(patchList);
                    HttpContent patchBody = new StringContent(serializedContent,
                    Encoding.UTF8, "application/json");
                    await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/light_bulbs/{lightBulb.Id}",
                    patchBody);
                }
            }
        }
        
        private async void AddLightBulb()
        {
            if (string.IsNullOrWhiteSpace(_newLightBulbName))
            {
                return;
            }
            HttpResponseMessage response = await Http.PostAsJsonAsync(
            $"houses/{_houseId}/rooms/{_roomId}/light_bulbs",
            new LightBulb()
            {
                Name = _newLightBulbName
            });
            LightBulb newLightBulb = await response.Content.ReadFromJsonAsync<LightBulb>();
            _lightBulbs.Add(newLightBulb);

            _newLightBulbName = string.Empty;
            StateHasChanged();
        }

        private async void DeleteLightBulb(Guid id)
        {
            await Http.DeleteAsync($"houses/{_houseId}/rooms/{_roomId}/light_bulbs/{id}");
            _lightBulbs.Remove(_lightBulbs.SingleOrDefault(lightBulb => lightBulb.Id == id));
            StateHasChanged();
        }

        private async void PatchLightBulbColor(Guid id)
        {
            LightBulb lightBulb = _lightBulbs.First(l => l.Id == id);
            int index = _lightBulbs.IndexOf(lightBulb);
            lightBulb.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbColorPatch(lightBulb.Color.Value));
            string serializedContent = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedContent,
            Encoding.UTF8,
            "application/json");
            await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/light_bulbs/{lightBulb.Id}",
            patchBody);
        }

        private async void SetWhiteColorAndPatchLightBulb(Guid id)
        {
            LightBulb lightBulb = _lightBulbs.First(l => l.Id == id);
            int index = _lightBulbs.IndexOf(lightBulb);
            _lightColors[index] = new LightColor("#FFFFFF");
            lightBulb.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbColorPatch(lightBulb.Color.Value));
            string serializedContent = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedContent,
            Encoding.UTF8,
            "application/json");
            await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/light_bulbs/{lightBulb.Id}",
            patchBody);
        }

        private async void SetOffColorAndPatchLightBulb(Guid id)
        {
            LightBulb lightBulb = _lightBulbs.First(l => l.Id == id);
            int index = _lightBulbs.IndexOf(lightBulb);
            _lightColors[index] = new LightColor("#000000");
            lightBulb.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbColorPatch(lightBulb.Color.Value));
            string serializedContent = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedContent,
            Encoding.UTF8,
            "application/json");
            await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/light_bulbs/{lightBulb.Id}",
            patchBody);
        }

        private async void PatchLightBulbIntensity(Guid id)
        {
            LightBulb lightBulb = _lightBulbs.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity ?? 0));
            string serializedContent = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedContent,
            Encoding.UTF8,
            "application/json");
            await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/light_bulbs/{lightBulb.Id}",
            patchBody);
        }

        private async void SetOffIntensityAndPatchLightBulb(Guid id)
        {
            LightBulb lightBulb = _lightBulbs.First(l => l.Id == id);
            lightBulb.Intensity = Byte.MinValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity.Value));
            string serializedContent = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedContent,
            Encoding.UTF8,
            "application/json");
            await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/light_bulbs/{lightBulb.Id}",
            patchBody);
        }

        private async void SetMaxIntensityAndPatchLightBulb(Guid id)
        {
            LightBulb lightBulb = _lightBulbs.First(l => l.Id == id);
            lightBulb.Intensity = Byte.MaxValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbIntensityPatch(lightBulb.Intensity.Value));
            string serializedContent = JsonConvert.SerializeObject(patchList);
            HttpContent patchBody = new StringContent(serializedContent,
            Encoding.UTF8,
            "application/json");
            await Http.PatchAsync($"houses/{_houseId}/rooms/{_roomId}/light_bulbs/{lightBulb.Id}",
            patchBody);
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