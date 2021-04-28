using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public abstract class BaseModel
    {
        [Required] public Guid Id { get; set; }
    }
}