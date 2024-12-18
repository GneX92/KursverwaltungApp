using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace Kursverwaltung.Models;

public class KursDbContext : DbContext
{
    public DbSet<Kurs> Kurse { get; set; }

    public KursDbContext( DbContextOptions options )
    : base( options ) { }
}