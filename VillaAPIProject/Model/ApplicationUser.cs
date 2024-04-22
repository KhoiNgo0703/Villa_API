using Microsoft.AspNetCore.Identity;

namespace VillaAPIProject.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
