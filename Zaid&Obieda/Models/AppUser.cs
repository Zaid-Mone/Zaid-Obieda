using Microsoft.AspNetCore.Identity;

namespace Zaid_Obieda.Models
{
    public class AppUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
    }
}
