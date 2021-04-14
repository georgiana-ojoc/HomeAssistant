using MediatR;

namespace API.Commands.House
{
    public class DeleteHouseCommand : IRequest<Shared.Models.House>
    {
        public string Email { get; set; }
        public int Id { get; set; }

        public DeleteHouseCommand(string email, int id)
        {
            Email = email;
            Id = id;
        }
    }
}