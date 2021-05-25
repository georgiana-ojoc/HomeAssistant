using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace API.Models
{
    public class LightBulbCommand : BaseModel
    {
        [Required] public Guid LightBulbId { get; set; }
        [Required] public Guid ScheduleId { get; set; }
        [Required] public int Color { get; set; }
        [Required] public byte Intensity { get; set; }

        internal LightBulb LightBulb { get; set; }
        internal Schedule Schedule { get; set; }
    }
}