namespace Library.Domain.Models
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
