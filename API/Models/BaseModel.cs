using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public abstract class BaseModel
    {
        [Required] public Guid Id { get; set; }
    }
}