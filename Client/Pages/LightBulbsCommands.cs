using System;
using System.Collections.Generic;
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
        private bool _addLightBulbCollapsed = true;
        private readonly IList<LightColor> _lightColors = new List<LightColor>();
        private Guid _newLightBulbCommandId = Guid.Empty;
        private IList<LightBulb> _lightBulbs = new List<LightBulb>();
        private IList<LightBulbCommandResponse> _lightBulbCommands;

        private async Task GetLightBulbs(Guid roomId)
        {
            _roomId = roomId;
            _newLightBulbCommandId = Guid.Empty;
            _lightBulbs = await _http.GetFromJsonAsync<IList<LightBulb>>($"{Path.Houses}/{_houseId}/" +
                                                                         $"{Path.Rooms}/{_roomId}/" +
                                                                         $"{Path.LightBulbs}");
        }

        private void SetNewLightBulbCommandId(Guid lightBulbId)
        {
            _newLightBulbCommandId = lightBulbId;
        }

        private async Task GetLightBulbCommands()
        {
            var response = await _http.GetFromJsonAsync<IList<LightBulbCommandResponse>>($"{Path.Schedules}/" +
                $"{_scheduleId}/{Path.LightBulbCommands}");
            if (response != null)
            {
                _lightBulbCommands = new List<LightBulbCommandResponse>(response);
            }

            foreach (var lightBulbCommand in _lightBulbCommands)
            {
                _lightColors.Add(new LightColor(lightBulbCommand.Color));
            }
        }

        private async Task AddLightBulbCommand()
        {
            if (_newLightBulbCommandId == Guid.Empty)
            {
                return;
            }

            var response = await _http.PostAsJsonAsync($"{Path.Schedules}/{_scheduleId}/" +
                                                       $"{Path.LightBulbCommands}",
                new LightBulbCommand
                {
                    LightBulbId = _newLightBulbCommandId,
                    Color = 0,
                    Intensity = byte.MinValue
                });
            if (response.IsSuccessStatusCode)
            {
                var newLightBulbCommand = await response.Content.ReadFromJsonAsync<LightBulbCommandResponse>();
                if (newLightBulbCommand != null)
                {
                    newLightBulbCommand = await _http.GetFromJsonAsync<LightBulbCommandResponse>(
                        $"{Path.Schedules}/{_scheduleId}/{Path.LightBulbCommands}/{newLightBulbCommand.Id}");
                    _lightBulbCommands.Add(newLightBulbCommand);
                }

                _lightColors.Add(new LightColor());
                _addLightBulbCollapsed = !_addLightBulbCollapsed;
                _houseId = Guid.Empty;
                _roomId = Guid.Empty;
                _newLightBulbCommandId = Guid.Empty;
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

        private async Task DeleteLightBulbCommand(Guid id)
        {
            await _http.DeleteAsync($"{Path.Schedules}/{_scheduleId}/{Path.LightBulbCommands}/{id}");
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
            await PatchCommand(patchList, Path.LightBulbCommands, lightBulbCommand.Id);
        }

        private async Task SetOnColorAndPatchLightBulbCommand(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            var index = _lightBulbCommands.IndexOf(lightBulbCommand);
            _lightColors[index] = new LightColor("#FFFFFF");
            lightBulbCommand.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandColorPatch(lightBulbCommand.Color));
            await PatchCommand(patchList, Path.LightBulbCommands, lightBulbCommand.Id);
        }

        private async Task SetOffColorAndPatchLightBulbCommand(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            var index = _lightBulbCommands.IndexOf(lightBulbCommand);
            _lightColors[index] = new LightColor("#000000");
            lightBulbCommand.Color = _lightColors[index].GetIntColor();
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandColorPatch(lightBulbCommand.Color));
            await PatchCommand(patchList, Path.LightBulbCommands, lightBulbCommand.Id);
        }

        private async Task PatchLightBulbCommandIntensity(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandIntensityPatch(lightBulbCommand.Intensity));
            await PatchCommand(patchList, Path.LightBulbCommands, lightBulbCommand.Id);
        }

        private async Task SetMinIntensityAndPatchLightBulbCommand(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            lightBulbCommand.Intensity = byte.MinValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandIntensityPatch(lightBulbCommand.Intensity));
            await PatchCommand(patchList, Path.LightBulbCommands, lightBulbCommand.Id);
        }

        private async Task SetMaxIntensityAndPatchLightBulbCommand(Guid id)
        {
            var lightBulbCommand = _lightBulbCommands.First(l => l.Id == id);
            lightBulbCommand.Intensity = byte.MaxValue;
            IList<Dictionary<string, string>> patchList = new List<Dictionary<string, string>>();
            patchList.Add(GenerateLightBulbCommandIntensityPatch(lightBulbCommand.Intensity));
            await PatchCommand(patchList, Path.LightBulbCommands, lightBulbCommand.Id);
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