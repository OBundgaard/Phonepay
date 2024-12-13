using UserService.Contexts;
using Phonepay.Core.Models;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Phonepay.Core.Interfaces;


namespace UserService.Repositories;

public class FriendshipRepository : IRepositoryById<Friendship>
{
    private readonly UserDbContext db;
    private readonly AsyncRetryPolicy retryPolicy;

    public FriendshipRepository(UserDbContext context)
    {
        db = context;
        retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<Friendship> Post(Friendship entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Post, save, and return
            await db.Friendships.AddAsync(entry);
            await db.SaveChangesAsync();
            return entry;
        });
    }

    public async Task<Friendship?> Get(int id)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and return
            var friendship = await db.Friendships.FindAsync(id);
            return friendship;
        });
    }

    public async Task<IEnumerable<Friendship>> GetAll(int userId)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Get all and return
            var friendships = await db.Friendships
                .Where(f => f.UserID == userId || f.FriendID == userId)
                .ToListAsync();
            return friendships;
        });
    }

    public async Task<Friendship> Put(Friendship entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Update, save, and return
            var friendship = db.Friendships.Update(entry).Entity;
            await db.SaveChangesAsync();
            return friendship;
        });
    }

    public async Task Delete(int id)
    {
        await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and verify existence
            var friendship = await db.Friendships.FindAsync(id);
            if (friendship == null)
                return;

            // Delete and save
            db.Friendships.Remove(friendship);
            await db.SaveChangesAsync();
        });
    }
}