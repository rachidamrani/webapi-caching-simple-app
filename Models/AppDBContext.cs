using Microsoft.EntityFrameworkCore;

public class AppDBContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public AppDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.LogTo(Console.WriteLine);
    }
}