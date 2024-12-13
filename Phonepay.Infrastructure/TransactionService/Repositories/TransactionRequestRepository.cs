using Phonepay.Core.Interfaces;
using Phonepay.Core.Models;
using Polly.Retry;
using Polly;
using TransactionService.Contexts;
using Microsoft.EntityFrameworkCore;

namespace TransactionService.Repositories;

public class TransactionRequestRepository : IRepositoryById<TransactionRequest>
{
    private readonly TransactionDbContext db;
    private readonly AsyncRetryPolicy retryPolicy;

    public TransactionRequestRepository(TransactionDbContext context)
    {
        db = context;
        retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<TransactionRequest> Post(TransactionRequest entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Post, save, and return
            await db.TransactionRequests.AddAsync(entry);
            await db.SaveChangesAsync();
            return entry;
        });
    }

    public async Task<TransactionRequest?> Get(int id)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and return
            var transactionRequest = await db.TransactionRequests.FindAsync(id);
            return transactionRequest;
        });
    }

    public async Task<IEnumerable<TransactionRequest>> GetAll(int userId)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Get all and return
            var transactionRequests = await db.TransactionRequests
                .Where(t => t.SenderID == userId || t.ReceiverID == userId)
                .ToListAsync();
            return transactionRequests;
        });
    }

    public async Task<TransactionRequest> Put(TransactionRequest entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Update, save, and return
            var transactionRequest = db.TransactionRequests.Update(entry).Entity;
            await db.SaveChangesAsync();
            return transactionRequest;
        });
    }

    public async Task Delete(int id)
    {
        await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and verify existence
            var transactionRequest = await db.TransactionRequests.FindAsync(id);
            if (transactionRequest == null)
                return;

            // Delete and save
            db.TransactionRequests.Remove(transactionRequest);
            await db.SaveChangesAsync();
        });
    }
}
