namespace Escola.API.DTO
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PrevPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
