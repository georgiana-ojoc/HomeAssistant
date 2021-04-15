using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Shared
{
    internal record IdRecord
    {
        public int HouseId { get; init; }
        public int RoomId { get; init; }
    }

    public class IdService
    {
        private readonly ILocalStorageService _localStorageService;

        public IdService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task SetHouseId(int houseId)
        {
            var idRecord = await GetIdRecord();
            var newIdRecord = idRecord with
            {
                HouseId = houseId
            };
            await _localStorageService.SetItemAsync("idRecord", newIdRecord);
        }

        public async Task SetRoomId(int roomId)
        {
            var idRecord = await GetIdRecord();
            var newIdRecord = idRecord with
            {
                RoomId = roomId
            };
            await _localStorageService.SetItemAsync("idRecord", newIdRecord);
        }

        public async Task<int> GetHouseId()
        {
            return (await GetIdRecord()).HouseId;
        }

        public async Task<int> GetRoomId()
        {
            return (await GetIdRecord()).RoomId;
        }

        private async Task<IdRecord> GetIdRecord()
        {
            return await _localStorageService.GetItemAsync<IdRecord>("idRecord") ?? new IdRecord();
        }
    }
}