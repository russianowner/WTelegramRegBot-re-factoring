using Core;
using Microsoft.EntityFrameworkCore;
namespace Data;

public class BotDbContext : DbContext
{
    public DbSet<Profile> Profiles => Set<Profile>();
    private readonly string _conn;
    public BotDbContext(string conn)
    {
        _conn = conn;
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite(_conn);
}
