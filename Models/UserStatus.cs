namespace UserManagement.Models
{
    public class UserStatus
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
