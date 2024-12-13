using Phonepay.Core.Interfaces;
using Phonepay.Core.Models;
using Polly.Retry;
using Polly;
using TransactionService.Contexts;
using Microsoft.EntityFrameworkCore;

namespace TransactionService.Repositories;

public class TransactionRepository : IRepositoryById<Transaction>
{
    private readonly TransactionDbContext db;
    private readonly AsyncRetryPolicy retryPolicy;

    public TransactionRepository(TransactionDbContext context)
    {
        db = context;
        retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<Transaction> Post(Transaction entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Post, save, and return
            await db.Transactions.AddAsync(entry);
            await db.SaveChangesAsync();
            return entry;
        });
    }

    public async Task<Transaction?> Get(int id)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and return
            var transaction = await db.Transactions.FindAsync(id);
            return transaction;
        });
    }

    public async Task<IEnumerable<Transaction>> GetAll(int userId)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Get all and return
            var transactions = await db.Transactions
                .Where(t => t.SenderID == userId || t.ReceiverID == userId)
                .ToListAsync();
            return transactions;
        });
    }

    public async Task<Transaction> Put(Transaction entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Update, save, and return
            var transaction = db.Transactions.Update(entry).Entity;
            await db.SaveChangesAsync();
            return transaction;
        });
    }

    public async Task Delete(int id)
    {
        await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and verify existence
            var transaction = await db.Transactions.FindAsync(id);
            if (transaction == null)
                return;

            // Delete and save
            db.Transactions.Remove(transaction);
            await db.SaveChangesAsync();
        });
    }
}
