using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Client.Models;
using Client.Utility;
using Microsoft.JSInterop;
using Radzen;

namespace Client.Pages
{
    public partial class Schedules
    {
        private bool _addScheduleCollapsed = true;
        private DateTime _newScheduleTime = DateTime.Parse(TimeSpan.Zero.ToString());

        private IEnumerable<int> _selectedDays = new List<int>
        {
            1
        };

        private Schedule _newSchedule = new()
        {
            Time = DateTime.Parse(TimeSpan.Zero.ToString()).ToString("HH:mm"),
            Days = 1
        };

        private static readonly IList<string> StringDays = new List<string>
        {
            "Mon",
            "Tue",
            "Wed",
            "Thu",
            "Fri",
            "Sat",
            "Sun"
        };

        private IList<Schedule> _schedules;

        protected override async Task OnInitializedAsync()
        {
            _schedules = await _http.GetFromJsonAsync<IList<Schedule>>($"{Path.Schedules}");
        }

        private async Task AddSchedule(Schedule newScheduleModel)
        {
            var response = await _http.PostAsJsonAsync($"{Path.Schedules}", _newSchedule);
            if (response.IsSuccessStatusCode)
            {
                var newSchedule = await response.Content.ReadFromJsonAsync<Schedule>();
                _schedules.Add(newSchedule);
                _addScheduleCollapsed = !_addScheduleCollapsed;
                _newSchedule = new Schedule
                {
                    Time = DateTime.Parse(TimeSpan.Zero.ToString()).ToString("HH:mm"),
                    Days = 1
                };
                _selectedDays = new List<int>
                {
                    1
                };
                StateHasChanged();
            }
            else
            {
                if (response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.PaymentRequired or
                    HttpStatusCode.Conflict)
                {
                    await _jsRuntime.InvokeVoidAsync("alert", await response.Content.ReadAsStringAsync());
                }
            }
        }

        private async Task DeleteSchedule(Guid id)
        {
            await _http.DeleteAsync($"{Path.Schedules}/{id}");
            _schedules.Remove(_schedules.SingleOrDefault(schedule => schedule.Id == id));
            StateHasChanged();
        }

        private async Task SetScheduleId(Guid id)
        {
            await _idService.SetScheduleId(id);
            _navManager.NavigateTo("commands");
        }

        private static string GetDays(int days)
        {
            int day = 0;
            StringBuilder result = new StringBuilder();
            while (days > 0 && day < StringDays.Count)
            {
                if (days % 2 == 1)
                {
                    result.Append($"{StringDays[day]} ");
                }

                days /= 2;
                day++;
            }

            return result.ToString();
        }

        private async Task OnInvalidSubmit(FormInvalidSubmitEventArgs arg)
        {
            await _jsRuntime.InvokeVoidAsync("alert", "Check your input and try again!");
        }

        private void OnChangeTime(DateTime? value, string format)
        {
            _newSchedule.Time = value?.ToString(format);
        }

        private void OnChangeSelectedDays(IEnumerable<int> selectedDays)
        {
            _newSchedule.Days = 0;
            foreach (var value in selectedDays)
            {
                _newSchedule.Days += (byte) value;
            }
        }
    }
}