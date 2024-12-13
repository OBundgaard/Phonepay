using Microsoft.EntityFrameworkCore;
using Phonepay.Core.Models;

namespace TransactionService.Contexts;

public class TransactionDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionRequest> TransactionRequests { get; set; }

    public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>().HasKey(t => t.ID);
        modelBuilder.Entity<Transaction>().Property(t => t.ID).ValueGeneratedOnAdd();

        modelBuilder.Entity<TransactionRequest>().HasKey(t => t.ID);
        modelBuilder.Entity<TransactionRequest>().Property(t => t.ID).ValueGeneratedOnAdd();
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