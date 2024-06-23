using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StoreManager.Data;
using StoreManager.Models;
using System.Transactions;

namespace StoreManager.DBAccess
{
    public class ChainDB : IChainDB
    {
        private readonly ApplicationDbContext _context;

        public ChainDB(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Chain>  CreateChain(Chain chain)
        {
             using var Transaction = _context.Database.BeginTransactionAsync();

            try
            {
                
                _context.Chains.Add(chain);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();
                return chain;
            }
            catch (DbUpdateException ex)
            {
                await _context.Database.RollbackTransactionAsync();
                
                foreach (var entry in _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
                {
                    entry.State = EntityState.Detached;
                }
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601) // Vi antager 2627 for unique constraint error i SQL Server
                {
                    throw new InvalidOperationException("Chain name must be unique.", ex);
                }
                throw;
            }
        }


        public async Task<Chain> GetChainById(Guid id)
        {
            var chain = await _context.Chains.FindAsync(id);
            return chain;
        }

        public async Task<List<Chain>> GetChains()
        {
            var chains = await _context.Chains.ToListAsync();
            return chains;
        }

        public async Task<Chain> UpdateChain(Chain chain)
        {
            // Check if the chain exists in the database
            var existingChain = await _context.Chains.FindAsync(chain.Id);

            if (existingChain == null)
            {
                throw new KeyNotFoundException("Chain not found");
            }

            // Update the existing chain with the new values
            existingChain.Name = chain.Name;
            existingChain.ModifiedOn = DateTime.Now;
            // ... Update other properties as needed

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return existingChain;
        }

        public async Task DeleteChain(Guid id)
        {
            var chain = await _context.Chains
                                      .Include(c => c.Stores)
                                      .FirstOrDefaultAsync(c => c.Id == id);

            if (chain == null)
            {
                throw new KeyNotFoundException("Chain not found");
            }

            if (chain.Stores.Any())
            {
                throw new InvalidOperationException("Cannot delete a chain that has associated stores.");
            }

            _context.Chains.Remove(chain);
            await _context.SaveChangesAsync();
        }

      
    }
}
