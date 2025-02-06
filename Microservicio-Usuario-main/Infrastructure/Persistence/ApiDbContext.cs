using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Configuracion con fluent Api
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserID);
                entity.Property(u => u.UserID)
                    .ValueGeneratedOnAdd();

                entity.Property(u => u.IsAdmin) 
                    .IsRequired();

                entity.Property(u => u.IsActive)
                    .IsRequired();

                entity.Property(u => u.Name)
                    .HasMaxLength(255)
                    .HasColumnType("varchar")
                    .IsRequired();

                entity.Property(u => u.Email)
                    .HasMaxLength(255)
                    .HasColumnType("varchar")
                    .IsRequired();

                entity.Property(u => u.Password)
                    .HasMaxLength(255)
                    .HasColumnType("varchar")
                    .IsRequired();

                entity.Property(u => u.ImageUrl)
                    .HasMaxLength(500)
                    .HasColumnType("varchar")
                    .IsRequired();
            });

            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(r => r.RefreshTokenID);
                entity.Property(r => r.RefreshTokenID)
                    .ValueGeneratedOnAdd();

                entity.Property(r => r.Token)
                    .HasColumnType("varchar(max)")
                    .IsRequired();

                entity.Property(r => r.CreateDate)
                    .IsRequired();

                entity.Property(r => r.ExpireDate)
                    .IsRequired();

                entity.Property(r => r.IsActive)
                    .IsRequired();

                entity.HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId);
            });
        }
    }
}
