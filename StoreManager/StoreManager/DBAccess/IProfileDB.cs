using System.Collections.Generic;
using System.Threading.Tasks;
using StoreManager.Models;

public interface IProfileDB
{
    Task<List<Profile>> GetProfiles();
    Task<Profile> GetProfileByEmail(string email);
    Task<Profile> CreateProfile(Profile profile);
    Task<Profile> UpdateProfile(Profile profile);
    Task DeleteProfile(string email);
}
