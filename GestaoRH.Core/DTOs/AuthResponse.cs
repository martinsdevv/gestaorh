namespace GestaoRH.Core.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
