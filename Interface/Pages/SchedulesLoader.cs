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

        private Schedule _newSchedule = new();

        private IEnumerable<int> _selectedDays = new List<int>();

        private async Task AddSchedule(Schedule newScheduleModel)
        {
            var response = await _http.PostAsJsonAsync("schedules", _newSchedule);
            if (response.IsSuccessStatusCode)
            {
                var newSchedule = await response.Content.ReadFromJsonAsync<Schedule>();
                _schedules.Add(newSchedule);

                _newSchedule = new Schedule();
                _selectedDays = new List<int>();
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

        private void OnChangeSelectedDays(IEnumerable<int> selectedDays)
        {
            _newSchedule.Days = 0;
            foreach (var value in selectedDays)
            {
                _newSchedule.Days += (byte) value;
            }
        }

        private void OnChangeTime(DateTime? value, string format)
        {
            _newSchedule.Time = value?.ToString(format);
        }
    }
}