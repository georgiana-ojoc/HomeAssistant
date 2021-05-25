using System;
using HomeAssistantAPI.Models;

#nullable disable

namespace HomeAssistantAPI.Responses
{
    public class LightBulbCommandResponse : BaseModel
    {
        public Guid ScheduleId { get; set; }
        public Guid LightBulbId { get; set; }
        public string LightBulbName { get; set; }
        public int Color { get; set; }
        public byte Intensity { get; set; }
    }
}