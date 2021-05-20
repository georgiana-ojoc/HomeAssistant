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
    public class DoorRepositoryTest : BaseRepositoryTest
    {
        #region GET_DOOR_ASYNC

        [Fact]
        public async void GivenRoomId_WhenRoomIdExists_ThenGetDoorsAsyncShouldReturnListOfDoors()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            var result = await repository.GetDoorsAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"));

            result.Should().BeOfType<List<Door>>();
        }

        [Fact]
        public async void GivenRoomId_WhenRoomIdDoesNotExist_ThenGetDoorsAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            var result = await repository.GetDoorsAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        [Fact]
        public async void GivenRoomId_WhenRoomIdIsEmpty_ThenGetDoorsAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.GetDoorsAsync("homeassistantgo@outlook.com",
                    Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                    Guid.Empty);
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Room id cannot be empty.");
        }

        #endregion

        #region GET_DOOR_BY_ID_ASYNC

        [Fact]
        public async void GivenDoor_WhenDoorExists_ThenGetDoorByIdAsyncShouldReturnDoor()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            var result = await repository.GetDoorByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"));

            result.Should().BeOfType<Door>();
        }

        [Fact]
        public async void GivenDoor_WhenDoorDoesNotExist_ThenGetDoorByIdAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            var result = await repository.GetDoorByIdAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"));

            result.Should().BeNull();
        }

        #endregion

        #region CREATE_DOOR_ASYNC

        [Fact]
        public async void GivenNewDoor_WhenDoorIsNotEmpty_ThenCreateDoorAsyncShouldReturnNewDoor()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            var result = await repository.CreateDoorAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"), new Door()
                {
                    Name = "Door"
                });

            result.Should().BeOfType<Door>();
        }

        [Fact]
        public async void
            GivenNewDoor_WhenDoorIsEmpty_ThenCreateDoorAsyncShouldThrowArgumentNullException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateDoorAsync("homeassistantgo@outlook.com",
                    Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                    Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"), new Door());
            };

            await function.Should().ThrowAsync<ArgumentNullException>()
                .WithMessage("Name cannot be empty.");
        }

        [Fact]
        public async void
            GivenNewDoor_WhenNameExists_ThenCreateDoorAsyncShouldThrowDuplicateNameException()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            Func<Task> function = async () =>
            {
                await repository.CreateDoorAsync("homeassistantgo@outlook.com",
                    Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                    Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"), new Door()
                    {
                        Name = "Balcony door"
                    });
            };

            await function.Should().ThrowAsync<DuplicateNameException>()
                .WithMessage("You already have a door with the specified name in this room.");
        }
        
        #endregion

        #region PARTIAL_UPDATE_DOOR_ASYNC

        [Fact]
        public async void
            GivenPartialUpdatedDoor_WhenDoorExists_ThenPartialUpdatedDoorAsyncShouldReturnPartialUpdatedDoor()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            JsonPatchDocument<DoorRequest> doorPatch = new JsonPatchDocument<DoorRequest>();
            doorPatch.Replace(d => d.Locked, true);

            var result = await repository.PartialUpdateDoorAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"), doorPatch);

            result.Should().BeOfType<Door>();
            result.Locked.Should().Be(true);
        }

        [Fact]
        public async void
            GivenPartialUpdatedDoor_WhenDoorDoesNotExist_ThenPartialUpdatedDoorAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            JsonPatchDocument<DoorRequest> doorPatch = new JsonPatchDocument<DoorRequest>();
            doorPatch.Replace(d => d.Locked, true);

            var result = await repository.PartialUpdateDoorAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("b5ce7683-968c-437d-8867-4a2bf4bab88b"), doorPatch);

            result.Should().BeNull();
        }

        #endregion

        #region DELETE_DOOR_ASYNC

        [Fact]
        public async void GivenDoor_WhenDoorExists_ThenDeleteDoorAsyncShouldReturnDoor()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            var result = await repository.DeleteDoorAsync("homeassistantgo@outlook.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"));

            result.Should().BeOfType<Door>();
        }

        [Fact]
        public async void GivenDoor_WhenDoorDoesNotExist_ThenDeleteDoorAsyncShouldReturnNull()
        {
            await using HomeAssistantContext context = GetContextWithData();
            IMapper mapper = GetMapper();
            DoorRepository repository = new DoorRepository(context, mapper);

            var result = await repository.DeleteDoorAsync("jane.doe@mail.com",
                Guid.Parse("cae88006-a2d7-4dcd-93fc-0b561e1f1acc"),
                Guid.Parse("f6ed4eb2-ac66-429b-8199-8757888bb0ad"),
                Guid.Parse("c4d7c02a-45ef-44ba-96ab-90c731db18ba"));

            result.Should().BeNull();
        }

        #endregion
    }
}