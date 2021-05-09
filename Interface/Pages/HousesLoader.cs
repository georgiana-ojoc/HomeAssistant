using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Houses
    {
        private IList<House> _houses;
        private string _newHouseName;

        private async Task AddHouse()
        {
            if (string.IsNullOrWhiteSpace(_newHouseName)) return;

            var response = await Http.PostAsJsonAsync("houses",
                new House
                {
                    Name = _newHouseName
                });
            if (response.IsSuccessStatusCode)
            {
                var newHouse = await response.Content.ReadFromJsonAsync<House>();
                _houses.Add(newHouse);

                _newHouseName = string.Empty;
                StateHasChanged();
            }
            else
            {
                await JsRuntime.InvokeVoidAsync("alert", "Maximum number of houses reached!");
            }
        }

        private async Task DeleteHouse(Guid id)
        {
            await Http.DeleteAsync($"houses/{id}");
            _houses.Remove(_houses.SingleOrDefault(house => house.Id == id));
            StateHasChanged();
        }

        private async Task SetHouseId(Guid id)
        {
            await IdService.SetHouseId(id);
            NavManager.NavigateTo("Rooms");
        }
    }
}