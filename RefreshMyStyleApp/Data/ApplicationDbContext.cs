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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet <Like> Likes { get; set; }

        public DbSet<Claim> Claims { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Notification> Notifications { get; set; }
       
        public DbSet<NotificationUser> UserNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
                .HasData(
                new IdentityRole
                {
                    Name = "ApplicationUser",
                    NormalizedName = "ApplicationUser"

                });

            builder.Entity<NotificationUser>()
                .HasKey(k => new { k.NotificationId, k.PersonId });

            builder.Entity<Friendship>()
                .Property(f => f.MeId);

        }


        
    }
}
