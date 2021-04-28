using System;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Shared.Models
{
    public class DoorCommand : BaseModel
    {
        [Required] public Guid DoorId { get; set; }
        [Required] public Guid ScheduleId { get; set; }
        [Required] public bool Locked { get; set; }

        internal Door Door { get; set; }
        internal Schedule Schedule { get; set; }
    }
}