using Microsoft.AspNetCore.Identity;

namespace StoreManager.DBAccess
{
    public interface IRoleDB
    {

        public Task<List<IdentityRole>> GetRolesAsync();
    }
}
