using Azure.Core.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2;

public partial class SovaLoggerContext : DbContext
{
    public DbSet<Log> Logs{ get; set; }

    public SovaLoggerContext(DbContextOptions<SovaLoggerContext> options)
        : base(options)
    {
        /*...*/
    }

#if DEBUG
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
#endif

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //GETDATE()
        // modelBuilder.Entity<YourEntity>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<Log>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<Log>()
            .Property<Guid>(e => e.Id)
            .HasDefaultValueSql("NEWID()");

        modelBuilder.Entity<Log>()
            .Property(e => e.TimeStamp)
            .HasDefaultValueSql("GETDATE()");
    }
}

public class Log
{
    public Guid Id { get; set; }
    public DateTime TimeStamp { get; set; } 
    public string? GmailUser { get; set; }
    public string? Action { get; set; }
    public string? Data { get; set; }
}