using System;
using System.ComponentModel.DataAnnotations;

namespace HomeAssistantAPI.Models
{
    public abstract class BaseModel
    {
        [Required] public Guid Id { get; set; }
    }
}