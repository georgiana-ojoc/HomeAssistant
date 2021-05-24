using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        private Schedule _newSchedule = new()
        {
            Time = DateTime.Parse(TimeSpan.Zero.ToString()).ToString("HH:mm"),
            Days = 1
        };

        private IEnumerable<int> _selectedDays = new List<int>
        {
            1
        };

        private DateTime _newScheduleTime = DateTime.Parse(TimeSpan.Zero.ToString());
        private bool _addScheduleCollapsed = true;

        private async Task AddSchedule(Schedule newScheduleModel)
        {
            var response = await _http.PostAsJsonAsync("schedules", _newSchedule);
            if (response.IsSuccessStatusCode)
            {
                var newSchedule = await response.Content.ReadFromJsonAsync<Schedule>();
                _schedules.Add(newSchedule);

                _newSchedule = new Schedule
                {
                    Time = DateTime.Parse(TimeSpan.Zero.ToString()).ToString("HH:mm"),
                    Days = 1
                };
                _selectedDays = new List<int>
                {
                    1
                };
                _addScheduleCollapsed = !_addScheduleCollapsed;
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

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    await _jsRuntime.InvokeVoidAsync("alert",
                        "Check your input and try again!\nMake sure you have selected at least 1 day to repeat on!");
                }
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

        private async Task OnInvalidSubmit(FormInvalidSubmitEventArgs arg)
        {
            await _jsRuntime.InvokeVoidAsync("alert", "Check input and try again!");
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