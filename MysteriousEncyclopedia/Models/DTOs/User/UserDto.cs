namespace MysteriousEncyclopedia.Models.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public int AccessFailedCount { get; set; }
    }
}
