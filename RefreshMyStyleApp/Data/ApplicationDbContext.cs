using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RefreshMyStyleApp.Models;

namespace RefreshMyStyleApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
    
        public DbSet<Image> Images { get; set; }
        public DbSet<Person> People { get; set; }

        public DbSet<Event> Events { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .HasData(
                new IdentityRole
                {
                    Name = "Person",
                    NormalizedName = "PERSON"
                });


            builder.Entity<Friendship>(f =>
            {
                f.HasKey(f => new { f.MeId, f.FriendId });
                f
                    .HasOne(f => f.Me)
                    .WithMany(u => u.Friends)
                    .HasForeignKey(f => f.MeId);

                f
                    .HasOne(f => f.Friend)
                    .WithMany(u => u.Friends)
                    .HasForeignKey(f => f.FriendId);
            });
        }
    }
}
