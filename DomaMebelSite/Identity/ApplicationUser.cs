using DomaMebelSite.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DomaMebelSite.Identity
{
    public class ApplicationUser : IdentityUser
    {   
        public ICollection<Project> Projects { get; set; }
    }
}
