using Microsoft.EntityFrameworkCore;
using WebApiProyecto.Models;

namespace WebApiProyecto.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Juego> Juegos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}