using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using API.Repositories;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Shared;
using Shared.Models;
using Shared.Requests;
using Xunit;

namespace Tests.RepositoryTests
{
    public class RoomRepositoryTest : BaseRepositoryTest
    {
        #region GET_ROOM_ASYNC

        [Fact]
        public async void GivenHouseId_WhenHouseIdExists_ThenGetRoomsAsyncShouldReturnListOfRooms()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            var result = await repository.GetRoomsAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"));

            result.Should().BeOfType<List<Room>>();
        }

        [Fact]
        public async void GivenHouseId_WhenHouseIdDoesNotExist_ThenGetRoomsAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            var result = await repository.GetRoomsAsync("homeassistantgo@outlook.com",
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenHouseId_WhenHouseIdIsEmpty_ThenGetRoomsAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.GetRoomsAsync("homeassistantgo@outlook.com",
                    Guid.Empty);
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("House id cannot be empty.");
        }

        #endregion

        #region GET_ROOM_BY_ID_ASYNC

        [Fact]
        public async void GivenRoom_WhenRoomExists_ThenGetRoomByIdAsyncShouldReturnRoom()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            var result = await repository.GetRoomByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"));

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenRoom_WhenRoomDoesNotExist_ThenGetRoomByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            var result = await repository.GetRoomByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_ROOM_ASYNC

        [Fact]
        public async void GivenNewRoom_WhenRoomIsNotEmpty_ThenCreateRoomAsyncShouldReturnNewRoom()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            var result = await repository.CreateRoomAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"), new Room()
                {
                    Name = "Bathroom"
                });

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenNewRoom_WhenRoomIsEmpty_ThenCreateRoomAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateRoomAsync("homeassistantgo@outlook.com",
                    Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"), new Room());
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Name cannot be empty.");
        }

        [Fact]
        public async void GivenNewRoom_WhenNameExists_ThenCreateRoomAsyncShouldThrowDuplicateNameException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateRoomAsync("homeassistantgo@outlook.com",
                    Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"), new Room()
                    {
                        Name = "Kitchen"
                    });
            };

            await function.Should().ThrowAsync<DuplicateNameException>()
                .WithMessage("You already have a room with the specified name in this house.");
        }

        #endregion

        #region PARTIAL_UPDATE_ROOM_ASYNC

        [Fact]
        public async void
            GivenPartialUpdatedRoom_WhenRoomExists_ThenPartialUpdatedRoomAsyncShouldReturnPartialUpdatedRoom()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            JsonPatchDocument<RoomRequest> roomPatch = new JsonPatchDocument<RoomRequest>();
            roomPatch.Replace(r => r.Name, "Bedroom");

            var result = await repository.PartialUpdateRoomAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"), roomPatch);

            result.Should().BeOfType<Room>();
            result.Name.Should().Be("Bedroom");
        }

        [Fact]
        public async void GivenPartialUpdatedRoom_WhenRoomDoesNotExist_ThenPartialUpdatedRoomAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            JsonPatchDocument<RoomRequest> roomPatch = new JsonPatchDocument<RoomRequest>();
            roomPatch.Replace(r => r.Name, "Lake room");

            var result = await repository.PartialUpdateRoomAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"), roomPatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_ROOM_ASYNC

        [Fact]
        public async void GivenRoom_WhenRoomExists_ThenDeleteRoomAsyncShouldReturnRoom()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            var result = await repository.DeleteRoomAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"));

            result.Should().BeOfType<Room>();
        }

        [Fact]
        public async void GivenRoom_WhenRoomDoesNotExist_ThenDeleteRoomAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            RoomRepository repository = new RoomRepository(context, mapper);

            var result = await repository.DeleteRoomAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion
    }
}