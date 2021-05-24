using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Shared.Models;

namespace Interface.Pages
{
    public partial class Houses
    {
        private IList<House> _houses;
        private House _newHouse = new();
        private bool _addHouseCollapsed = true;

        private async Task AddHouse()
        {
            var response = await _http.PostAsJsonAsync("houses", _newHouse);
            if (response.IsSuccessStatusCode)
            {
                var newHouse = await response.Content.ReadFromJsonAsync<House>();
                _houses.Add(newHouse);

                _newHouse = new House();
                _addHouseCollapsed = !_addHouseCollapsed;
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
            }
        }

        private async Task DeleteHouse(Guid id)
        {
            await _http.DeleteAsync($"houses/{id}");
            _houses.Remove(_houses.SingleOrDefault(house => house.Id == id));
            StateHasChanged();
        }

        private async Task SetHouseId(Guid id)
        {
            await _idService.SetHouseId(id);
            _navManager.NavigateTo("Rooms");
        }
    }
}