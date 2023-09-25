using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Azure_WebApi.Entities
{
    public class AppUser : IdentityUser
    {
        public ICollection<Clan> UserClans { get; set; } = new Collection<Clan>();
    }
}