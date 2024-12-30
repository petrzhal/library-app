namespace Library.Domain.Models
{
    public class RefreshToken : BaseModel
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
