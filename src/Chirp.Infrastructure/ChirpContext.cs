using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace Chirp.Infrastructure;

/// <summary>
/// The class 'ChirpContext' inherits from 'IdentityDbContext<Author, IdentityRole<int>, int>' which is a base class from 
/// Entity FrameWork Core used for managing the user Authentication. 
/// The class declares three databases which represents 'Cheeps', 'Author', and 'Likes'. These are used to query and 
/// save instances of these the given types.  
/// </summary>

public class ChirpContext : IdentityDbContext<Author, IdentityRole<int>, int>
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Like> Likes { get; set; }


    // This class constructor takes 'DbContextOptions' from the Entity FrameWork Core as an argument, to setup the context. 
    public ChirpContext(DbContextOptions<ChirpContext> options)
        : base(options)
    {
        
    }

    //Configures the schema for the Identity Framework with the given types. 
    // The 'ModelBuilder' argument is the API which defines the shape of the relationships and how they map to the database. 
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Like>().ToTable("Likes").HasNoKey();
        builder.Entity<Like>().HasKey(l => new {l.CheepId, l.AuthorId});

        builder.Entity<Like>().HasOne(l => l.Cheep).WithMany(c => c.Likes).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Like>().HasOne(l => l.Author).WithMany(c => c.Likes).OnDelete(DeleteBehavior.Restrict);
        

    }
}