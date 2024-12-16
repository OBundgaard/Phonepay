using UserService.Contexts;
using Phonepay.Core.Models;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Phonepay.Core.Interfaces;


namespace UserService.Repositories;

public class UserRepository : IBaseRepository<User>
{
    private readonly UserDbContext db;
    private readonly AsyncRetryPolicy retryPolicy;

    public UserRepository(UserDbContext context)
    {
        db = context;
        retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public async Task<User> Post(User entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Post, save, and return
            await db.Users.AddAsync(entry);
            await db.SaveChangesAsync();
            return entry;
        });
    }

    public async Task<User?> Get(int id)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and return
            var user = await db.Users.FindAsync(id);
            return user;
        });
    }

    public async Task<User> Put(User entry)
    {
        return await retryPolicy.ExecuteAsync(async () =>
        {
            // Update, save, and return
            var user = await db.Users.FindAsync(entry);

            user!.PhoneNumber = entry.PhoneNumber;
            user.Name = entry.Name;
            user.CreatedDate = entry.CreatedDate;

            await db.SaveChangesAsync();
            return user;
        });
    }

    public async Task Delete(int id)
    {
        await retryPolicy.ExecuteAsync(async () =>
        {
            // Get and verify existence
            var user = await db.Users.FindAsync(id);
            if (user == null)
                return;

            // Delete and save
            db.Users.Remove(user);
            await db.SaveChangesAsync();
        });
    }
}