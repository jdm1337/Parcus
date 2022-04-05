namespace Parcus.Domain.DTO.Outgoing
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
       
        
    }
}
