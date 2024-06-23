using StoreManager.Models;

namespace StoreManager.DBAccess
{
    public interface IChainDB
    {
        public  Task<List<Chain>> GetChains();
        public Task<Chain> CreateChain(Chain chain);
        public Task<Chain> UpdateChain(Chain chain);
        public Task DeleteChain(Guid id);
        public Task<Chain> GetChainById(Guid id);
    }
}
