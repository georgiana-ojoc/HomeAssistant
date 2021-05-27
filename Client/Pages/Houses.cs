using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Client.Models;
using Client.Utility;
using Microsoft.JSInterop;

namespace Client.Pages
{
    public partial class Houses
    {
        private bool _addHouseCollapsed = true;
        private House _newHouse = new();
        private IList<House> _houses;

        protected override async Task OnInitializedAsync()
        {
            _houses = await _http.GetFromJsonAsync<IList<House>>("houses");
        }

        private async Task AddHouse()
        {
            var response = await _http.PostAsJsonAsync($"{Path.Houses}", _newHouse);
            if (response.IsSuccessStatusCode)
            {
                var newHouse = await response.Content.ReadFromJsonAsync<House>();
                _houses.Add(newHouse);
                _addHouseCollapsed = !_addHouseCollapsed;
                _newHouse = new House();
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

        private async Task DeleteHouse(Guid id)
        {
            await _http.DeleteAsync($"{Path.Houses}/{id}");
            _houses.Remove(_houses.SingleOrDefault(house => house.Id == id));
            StateHasChanged();
        }

        private async Task SetHouseId(Guid id)
        {
            await _idService.SetHouseId(id);
            _navManager.NavigateTo($"{Path.Rooms}");
        }
    }
}