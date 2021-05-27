#nullable disable

namespace Client.Models
{
    public class Schedule : BaseModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public byte Days { get; set; }
    }
}