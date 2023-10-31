using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Chirp.Infrastructure;


public class ChirpContext : IdentityDbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }


    public ChirpContext(DbContextOptions<ChirpContext> options)
        : base(options)
    {
    }
}