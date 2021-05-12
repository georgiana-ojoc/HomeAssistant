using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Requests
{
    public class DoorCommandRequest
    {
        [Required] public Guid DoorId { get; set; }
        [Required] public bool Locked { get; set; }
    }
}