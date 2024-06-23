using StoreManager.Models;

namespace StoreManager.DBAccess
{
    public interface IStoreDB
    {
        public Task<List<Store>> GetStores();
        public Task<Store> CreateStore(Store Store);
        public Task<Store> UpdateStore(Store Store);
        public Task DeleteStore(Guid id);
        public Task<Store> GetStoreById(Guid id);
    }
}
