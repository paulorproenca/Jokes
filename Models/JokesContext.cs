using Microsoft.EntityFrameworkCore;

namespace Jokes.Models;

public class JokesContext : DbContext
{
    public JokesContext(DbContextOptions<JokesContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Joke> Jokes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .Property(t => t.Name)
            .HasMaxLength(15)
            .IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
                .LogTo(Console.WriteLine)		
                .EnableSensitiveDataLogging()       //to log sensitive data
                // .EnableDetailedErrors()          //get detailed query exceptions
                ;

}