using System.ComponentModel.DataAnnotations;

namespace API.Requests
{
    public class HouseRequest
    {
        [Required] public string Name { get; set; }
    }
}