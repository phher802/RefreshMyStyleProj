using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RefreshMyStyleApp.Models;
using RefreshMyStyleApp.ViewModels;


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
        public DbSet<Friend> Friends { get; set; }
        public DbSet<LikedItem> LikedItems { get; set; }
        public DbSet<ClaimedItem> ClaimedItems { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<AttendEvent> Attendees { get; set; }   
       
        public DbSet<Message> Messages { get; set; }
    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<IdentityRole>()
                .HasData(
                new IdentityRole
                {
                    Name = "ApplicationUser",
                    NormalizedName = "APPLICATIONUSER"

                });

     
                
         

        }
    
        public DbSet<RefreshMyStyleApp.ViewModels.EventViewModel> EventViewModel { get; set; }



    }
}
