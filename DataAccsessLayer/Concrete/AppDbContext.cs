using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccsessLayer.Concrete
{
    public class AppDbContext : IdentityDbContext<Dt_ApplicationUser, Dt_ApplicationRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dt_Appointment>()
                .HasOne(a => a.Shop)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dt_Appointment>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dt_Appointment>()
                .HasOne(a => a.Worker)
                .WithMany(w => w.Appointments)
                .HasForeignKey(a => a.WorkerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dt_Appointment>()
                .HasOne(a => a.TimeSlot)
                .WithMany(t => t.Appointments)
                .HasForeignKey(a => a.TimeSlotId)
                .OnDelete(DeleteBehavior.Restrict);

           
        }

        public DbSet<Dt_ApplicationRole> Dt_ApplicationRoles { get; set; }
        public DbSet<Dt_ApplicationUser> Dt_ApplicationUsers { get; set; }
        public DbSet<Dt_Appointment> Dt_Appointments { get; set; }
        public DbSet<Dt_AppointmentProduct> Dt_AppointmentProducts { get; set; }
        public DbSet<Dt_AppointmentService> Dt_AppointmentServices { get; set; }
        public DbSet<Dt_Discount> Dt_Discounts { get; set; }
        public DbSet<Dt_Notification> Dt_Notifications { get; set; }
        public DbSet<Dt_Payment> Dt_Payments { get; set; }
        public DbSet<Dt_Product> Dt_Products { get; set; }
        public DbSet<Dt_ProductCategory> Dt_ProductCategorys { get; set; }
        public DbSet<Dt_Review> Dt_Reviews { get; set; }
        public DbSet<Dt_Service> Dt_Services { get; set; }
        public DbSet<Dt_Shop> Dt_Shops { get; set; }
        public DbSet<Dt_ShopOwner> Dt_ShopOwners { get; set; }
        public DbSet<Dt_TimeSlots> Dt_TimeSlots { get; set; }
        public DbSet<Dt_Worker> Dt_Workers { get; set; }

        public DbSet<Dt_OtherUser> Dt_OtherUser { get; set; }
    }
}
