using Microsoft.EntityFrameworkCore;
using Phonepay.Core.Models;

namespace UserService.Contexts;

public class UserDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.ID);
        modelBuilder.Entity<User>().Property(u => u.ID).ValueGeneratedOnAdd();

        modelBuilder.Entity<Friendship>().HasKey(f => f.ID);
        modelBuilder.Entity<Friendship>().Property(f => f.ID).ValueGeneratedOnAdd();

        modelBuilder.Entity<FriendRequest>().HasKey(f => f.ID);
        modelBuilder.Entity<FriendRequest>().Property(f => f.ID).ValueGeneratedOnAdd();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("An error occurred while saving changes to the database.", ex);
        }
    }
}