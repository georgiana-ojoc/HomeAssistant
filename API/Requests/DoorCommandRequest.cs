using System;
using System.ComponentModel.DataAnnotations;

namespace API.Requests
{
    public class DoorCommandRequest
    {
        [Required] public Guid DoorId { get; set; }
        [Required] public bool Locked { get; set; }
    }
}