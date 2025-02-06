using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ApiDbContext : DbContext
    {
        //Hacer los DbSet de las clases
        public ApiDbContext() { }
       

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Reservation> Reservations { get; set; }

        public DbSet<ReservationStatus> ReservationsStatus { get; set; }

        public DbSet<Players> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ReservationMS;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True");
        }




        //Configurar la bd con fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.ReservationID);
                entity.Property(r => r.ReservationID).ValueGeneratedOnAdd();
                entity.Property(r => r.Date).IsRequired();
                entity.Property(r => r.StartTime).IsRequired();
                entity.Property(r => r.EndTime).IsRequired();
                entity.HasOne(r => r.StatusNavigator).WithMany(rs => rs.Reservations).HasForeignKey(r => r.ReservationStatusID);

            });

            modelBuilder.Entity<ReservationStatus>(entity =>
            {
                entity.HasKey(rs => rs.Id);
                entity.Property(rs => rs.Status).IsRequired().HasColumnType("varchar").HasMaxLength(25);

                entity.HasData(
                    new ReservationStatus { Id = 1, Status = "Reserved"},
                    new ReservationStatus { Id = 2, Status = "Finished" },
                    new ReservationStatus { Id = 3, Status = "Cancelled" }
                    );
            });

            modelBuilder.Entity<Players>(entity =>
            {
                entity.HasKey(p => new { p.ReservationID, p.UserID });
                entity.HasOne(p => p.Reservation).WithMany(r => r.Players).HasForeignKey(p => p.ReservationID);
                
            });
                

            
                
        }
    }
}
