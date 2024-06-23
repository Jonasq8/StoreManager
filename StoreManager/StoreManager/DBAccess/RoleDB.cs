using Microsoft.AspNetCore.Identity;

namespace StoreManager.DBAccess
{
    public class RoleDB : IRoleDB
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleDB(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            return  _roleManager.Roles.ToList();
        }
    }
}
