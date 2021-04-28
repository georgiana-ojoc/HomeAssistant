using System;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace Interface.Scripts
{
    internal record IdRecord
    {
        public Guid HouseId { get; init; }
        public Guid RoomId { get; init; }
    }

    public class IdService
    {
        private readonly ILocalStorageService _localStorageService;

        public IdService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task SetHouseId(Guid houseId)
        {
            var idRecord = await GetIdRecord();
            var newIdRecord = new IdRecord()
            {
                HouseId = houseId,
                RoomId = idRecord.RoomId
            };
            await _localStorageService.SetItemAsync("idRecord", newIdRecord);
        }

        public async Task SetRoomId(Guid roomId)
        {
            var idRecord = await GetIdRecord();
            var newIdRecord = new IdRecord()
            {
                HouseId = idRecord.HouseId,
                RoomId = roomId
            };
            await _localStorageService.SetItemAsync("idRecord", newIdRecord);
        }

        public async Task<Guid> GetHouseId()
        {
            return (await GetIdRecord()).HouseId;
        }

        public async Task<Guid> GetRoomId()
        {
            return (await GetIdRecord()).RoomId;
        }

        private async Task<IdRecord> GetIdRecord()
        {
            return await _localStorageService.GetItemAsync<IdRecord>("idRecord") ?? new IdRecord();
        }
    }
}