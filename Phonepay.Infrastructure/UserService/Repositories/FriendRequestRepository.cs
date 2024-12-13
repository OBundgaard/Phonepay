using UserService.Contexts;
using Phonepay.Core.Models;
using Phonepay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;


namespace UserService.Repositories;

public class FriendRequestRepository : IRepositoryById<FriendRequest>
{
    private readonly UserDbContext db;
    private readonly AsyncRetryPolicy retryPolicy;

    public FriendRequestRepository(UserDbContext context)
    {
        db = context;
        retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<FriendRequest> Post(FriendRequest entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Post, save, and return
            await db.FriendRequests.AddAsync(entry);
            await db.SaveChangesAsync();
            return entry;
        });
    }

    public async Task<FriendRequest?> Get(int id)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and return
            var friendRequest = await db.FriendRequests.FindAsync(id);
            return friendRequest;
        });
    }

    public async Task<IEnumerable<FriendRequest>> GetAll(int userId)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Get all and return
            var friendRequests = await db.FriendRequests
                .Where(f => f.SenderID == userId || f.ReceiverID == userId)
                .ToListAsync();
            return friendRequests;
        });
    }

    public async Task<FriendRequest> Put(FriendRequest entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Update, save, and return
            var friendRequest = db.FriendRequests.Update(entry).Entity;
            await db.SaveChangesAsync();
            return friendRequest;
        });
    }

    public async Task Delete(int id)
    {
        await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and verify existence
            var friendRequest = await db.FriendRequests.FindAsync(id);
            if (friendRequest == null)
                return;

            // Delete and save
            db.FriendRequests.Remove(friendRequest);
            await db.SaveChangesAsync();
        });
    }
}