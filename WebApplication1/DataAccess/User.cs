using Microsoft.AspNetCore.Identity;

namespace WebApplication1.DataAccess
{
    public class User : IdentityUser
    {
        public bool IsActivated { get; set; }
        public int Key { get; set; }
    }
}
