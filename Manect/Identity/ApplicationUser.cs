using Microsoft.AspNetCore.Identity;

namespace Manect.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [ProtectedPersonalData]
        public string FirstName { get; set; }
        [ProtectedPersonalData]
        public string LastName { get; set; }
    }
}
