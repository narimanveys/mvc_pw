using Microsoft.AspNetCore.Identity;

namespace PWApp.Models
{
    public class User : IdentityUser
    {
        public string FullName  { get; set; }
    }
   
}