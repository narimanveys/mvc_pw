namespace PWApp.Models
{
    public class UserAccount : BaseEntity
    {
        public decimal AvailableAmount { get; set; }
        public User User { get; set; }
    }
}