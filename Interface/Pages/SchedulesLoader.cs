using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Radzen;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Scheduler
    {
        private IList<Schedule> _schedules;

        private DateTime _newScheduleTime = DateTime.Now;

        private Schedule _newSchedule = new();

        private async Task AddSchedule(Schedule _newSchedule)
        {
            // if (string.IsNullOrWhiteSpace(_newScheduleName)) return;

            var response = await _http.PostAsJsonAsync("schedules", _newSchedule);
            if (response.IsSuccessStatusCode)
            {
                var newSchedule = await response.Content.ReadFromJsonAsync<Schedule>();
                _schedules.Add(newSchedule);

                this._newSchedule = new Schedule();
                StateHasChanged();
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("alert", "Maximum number of schedules reached!");
            }
        }

        private async Task DeleteSchedule(Guid id)
        {
            await _http.DeleteAsync($"schedules/{id}");
            _schedules.Remove(_schedules.SingleOrDefault(schedule => schedule.Id == id));
            StateHasChanged();
        }

        private async Task SetScheduleId(Guid id)
        {
            await _idService.SetScheduleId(id);
            _navManager.NavigateTo("ScheduleEditor");
        }
        private Task OnInvalidSubmit(FormInvalidSubmitEventArgs arg)
        {
            throw new NotImplementedException();
        }
    }
}