using System;
using HomeAssistantAPI.Models;

#nullable disable

namespace HomeAssistantAPI.Responses
{
    public class DoorCommandResponse : BaseModel
    {
        public Guid ScheduleId { get; set; }
        public Guid DoorId { get; set; }
        public string DoorName { get; set; }
        public bool Locked { get; set; }
    }
}