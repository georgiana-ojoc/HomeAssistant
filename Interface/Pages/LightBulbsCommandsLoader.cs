using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Interface.Scripts;
using Microsoft.JSInterop;
using Shared.Models;

namespace Interface.Pages
{
    public partial class ScheduleEditor
    {
        private readonly IList<LightColor> _lightColors = new List<LightColor>();
        private IList<LightBulbCommand> _lightBulbCommands;
        private Guid _newCommandLightBulbId = Guid.Empty;
        private IList<LightBulb> _lightBulbs = new List<LightBulb>();

        private async Task GetLightBulbs(Guid roomId)
        {
            _newCommandLightBulbId = Guid.Empty;
            _roomId = roomId;
            _lightBulbs = await _http.GetFromJsonAsync<IList<LightBulb>>(
                $"houses/{_houseId}/rooms/{_roomId}/{Paths.LightBulbsPath}");
        }

        private void SetNewCommandLightBulbId(Guid lightBulbId)
        {
            _newCommandLightBulbId = lightBulbId;
        }

        private async Task GetLightBulbCommands()
        {
            var responseLightBulbs = await _http.GetFromJsonAsync<IList<LightBulbCommand>>(
                $"schedules/{_scheduleId}/{Paths.LightBulbCommandsPath}");
            if (responseLightBulbs != null)
                _lightBulbCommands = new List<LightBulbCommand>(responseLightBulbs);
            foreach (var lightBulb in _lightBulbCommands)
            {
                _lightColors.Add(new LightColor(lightBulb.Color));
            }
        }

        private async Task AddLightBulbCommand()
        {
            if (_newCommandLightBulbId == Guid.Empty) return;

            var response = await _http.PostAsJsonAsync(
                $"schedules/{_scheduleId}/{Paths.LightBulbCommandsPath}",
                new LightBulbCommand
                {
                    LightBulbId = _newCommandLightBulbId,
                    Color = 0,
                    Intensity = byte.MinValue
                });
            if (response.IsSuccessStatusCode)
            {
                var newLightBulbCommand = await response.Content.ReadFromJsonAsync<LightBulbCommand>();
                _lightBulbCommands.Add(newLightBulbCommand);
                _lightColors.Add(new LightColor());

                _houseId = Guid.Empty;
                _roomId = Guid.Empty;
                _newCommandLightBulbId = Guid.Empty;
                StateHasChanged();
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("alert", "Maximum number of light bulbs reached!");
            }
        }

        private async Task DeleteLightBulbCommand(Guid id)
        {
            await _http.DeleteAsync($"schedules/{_scheduleId}/{Paths.LightBulbCommandsPath}/{id}");
            _lightBulbCommands.Remove(_lightBulbCommands.SingleOrDefault(lightBulb => lightBulb.Id == id));
            StateHasChanged();
        }

        private async Task PatchLightBulbCommandColor(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            var index = _lightBulbCommands.IndexOf(lightBulbCommand);
            lightBulbCommand.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandColorPatch(lightBulbCommand.Color));
            await PatchCommand(patchList, Paths.LightBulbCommandsPath, lightBulbCommand.Id);
        }

        private async Task SetWhiteColorAndPatchLightBulbCommand(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            var index = _lightBulbCommands.IndexOf(lightBulbCommand);
            _lightColors[index] = new LightColor("#FFFFFF");
            lightBulbCommand.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandColorPatch(lightBulbCommand.Color));
            await PatchCommand(patchList, Paths.LightBulbCommandsPath, lightBulbCommand.Id);
        }

        private async Task SetOffColorAndPatchLightBulbCommand(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            var index = _lightBulbCommands.IndexOf(lightBulbCommand);
            _lightColors[index] = new LightColor("#000000");
            lightBulbCommand.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandColorPatch(lightBulbCommand.Color));
            await PatchCommand(patchList, Paths.LightBulbCommandsPath, lightBulbCommand.Id);
        }

        private async Task PatchLightBulbCommandIntensity(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandIntensityPatch(lightBulbCommand.Intensity));
            await PatchCommand(patchList, Paths.LightBulbCommandsPath, lightBulbCommand.Id);
        }

        private async Task SetOffIntensityAndPatchLightBulbCommand(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            lightBulbCommand.Intensity = byte.MinValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandIntensityPatch(lightBulbCommand.Intensity));
            await PatchCommand(patchList, Paths.LightBulbCommandsPath, lightBulbCommand.Id);
        }

        private async Task SetMaxIntensityAndPatchLightBulbCommand(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            lightBulbCommand.Intensity = byte.MaxValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandIntensityPatch(lightBulbCommand.Intensity));
            await PatchCommand(patchList, Paths.LightBulbCommandsPath, lightBulbCommand.Id);
        }

        private static Dictionary<string, string> GenerateLightBulbCommandColorPatch(int color)
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

        private static Dictionary<string, string> GenerateLightBulbCommandIntensityPatch(byte intensity)
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