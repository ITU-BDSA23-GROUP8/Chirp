using Microsoft.EntityFrameworkCore;
using System;
namespace Chirp.Infrastructure;


public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }


    public ChirpContext(DbContextOptions<ChirpContext> options)
        : base(options)
    {
    }
}