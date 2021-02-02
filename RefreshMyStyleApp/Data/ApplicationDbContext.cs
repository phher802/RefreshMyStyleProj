﻿using System;
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
        public DbSet<ProfileImage> ProfileImages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ClothingEnthusiast> ClothingEnthusiasts { get; set; }

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
                    Name = "ClothingEnthusiast",
                    NormalizedName = "CLOTHINGENTHUSIAST"
                });
        }
    }
}
