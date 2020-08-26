using Manect.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Manect.Identity
{
    public class ApplicationUser : IdentityUser
    {   
        public ICollection<Project> Projects { get; set; }
    }
}
