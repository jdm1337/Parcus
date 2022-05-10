namespace Parcus.Api.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public List<string> Permissions { get; set; }
        public int InstrumentsCount { get; set; }  
    }
}
