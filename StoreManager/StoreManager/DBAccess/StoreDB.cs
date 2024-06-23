using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreManager.Data;
using StoreManager.Models;

namespace StoreManager.DBAccess
{
    public class StoreDB : IStoreDB
    {
        private readonly ApplicationDbContext _context;

        public StoreDB(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Store> CreateStore(Store Store)
        {
            try
            {
                _context.Stores.Add(Store);
                await _context.SaveChangesAsync();
                return Store;
            }
            catch (DbUpdateException ex)
            {
              
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601) // Vi antager 2627 for unique constraint error i SQL Server
                {
                    throw new InvalidOperationException("Store NUMBER must be unique.", ex);
                }
                throw;
            }
        }

        public async Task DeleteStore(Guid id)
        {
            var store = await GetStoreById(id);
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }

        public async Task<Store> GetStoreById(Guid id)
        {
            var store = await _context.Stores.FindAsync(id);
            return store;
        }

        public async Task<List<Store>> GetStores()
        {
            var stores = await _context.Stores.ToListAsync();
            return stores;
        }

        public async Task<Store> UpdateStore(Store store)
        {
            var existingStore = await GetStoreById(store.Id);
            if (existingStore == null)
            {
                throw new KeyNotFoundException("Store not found");
            }

            // Opdater de nødvendige felter
            existingStore.Name = store.Name;
            existingStore.Address = store.Address;
            existingStore.PostalCode = store.PostalCode;
            existingStore.City = store.City;
            existingStore.Phone = store.Phone;
            existingStore.EMail = store.EMail;
            existingStore.StoreOwner = store.StoreOwner;
            existingStore.ModifiedOn = DateTime.Now; // Opdater ModifiedOn til nuværende tid

            // Gem ændringerne
            await _context.SaveChangesAsync();

            return existingStore;
        }
    }
}
