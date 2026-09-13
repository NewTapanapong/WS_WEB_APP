using Microsoft.EntityFrameworkCore;
using TodoApi.model;

namespace TodoApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Todoitem> Todos => Set<Todoitem>();
}
