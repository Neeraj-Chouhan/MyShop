using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser> FindByEmailAddressAsync(this UserManager<AppUser> input,
        ClaimsPrincipal user){
            
             var email = user?.Claims?.FirstOrDefault(x=> x.Type == ClaimTypes.Email)?.Value;

             return await input.Users.Include(x=>x.Address).SingleOrDefaultAsync(e=>e.Email == email);

        }
    }
}