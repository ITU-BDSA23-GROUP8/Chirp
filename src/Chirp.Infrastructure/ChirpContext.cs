using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace Chirp.Infrastructure;


public class ChirpContext : IdentityDbContext<Author, IdentityRole<int>, int>
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Like> Likes { get; set; }


    public ChirpContext(DbContextOptions<ChirpContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Like>().ToTable("Likes").HasNoKey();
        builder.Entity<Like>().HasKey(l => new {l.CheepId, l.AuthorId});

        builder.Entity<Like>().HasOne(l => l.Cheep).WithMany(c => c.Likes).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Like>().HasOne(l => l.Author).WithMany(c => c.Likes).OnDelete(DeleteBehavior.Restrict);
        

    }
}