using Microsoft.AspNetCore.Identity;
using StoreManager.Data;
using StoreManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProfileDB : IProfileDB
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ProfileDB(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<Profile>> GetProfiles()
    {
        var users = _userManager.Users.ToList();
        var profiles = new List<Profile>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault(); // Assuming a user has only one role
            profiles.Add(new Profile
            {
                EMail = user.Email,
                Name = user.Name,
                Role = role,
                CreatedOn = user.CreatedOn
            });
        }

        return profiles;
    }

    public async Task<Profile> GetProfileByEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault(); // Assuming a user has only one role

        return new Profile
        {
            EMail = user.Email,
            Name = user.Name,
            Role = role,
            CreatedOn = user.CreatedOn
        };
    }

    public async Task<Profile> CreateProfile(Profile profile)
    {
        var user = new ApplicationUser
        {
            UserName = profile.EMail,
            Email = profile.EMail,
            Name = profile.Name,
            CreatedOn = DateTime.Now,
            Role = profile.Role,
            
        };

        var result = await _userManager.CreateAsync(user, profile.Password);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (!await _roleManager.RoleExistsAsync(profile.Role))
        {
            await _roleManager.CreateAsync(new IdentityRole(profile.Role));
        }
        await _userManager.AddToRoleAsync(user, profile.Role);

        return profile;
    }

    public async Task<Profile> UpdateProfile(Profile profile)
    {
        var user = await _userManager.FindByEmailAsync(profile.EMail);
        if (user == null) return null;

        user.Name = profile.Name;

        var currentRoles = await _userManager.GetRolesAsync(user);
        var currentRole = currentRoles.FirstOrDefault();

        if (currentRole != profile.Role)
        {
            await _userManager.RemoveFromRoleAsync(user, currentRole);
            if (!await _roleManager.RoleExistsAsync(profile.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(profile.Role));
            }
            await _userManager.AddToRoleAsync(user, profile.Role);
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return profile;
    }

    public async Task DeleteProfile(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
       
        if (user != null && user.Name != "Admin")
        {


            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }


        }
        else
        {
            throw new Exception("Admin can't be deleted");
        }
    }
}
