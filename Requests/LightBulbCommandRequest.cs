using System;
using System.ComponentModel.DataAnnotations;

namespace HomeAssistantAPI.Requests
{
    public class LightBulbCommandRequest
    {
        [Required] public Guid LightBulbId { get; set; }
        [Required] public int Color { get; set; }
        [Required] public byte Intensity { get; set; }
    }
}