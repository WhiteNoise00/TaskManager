

using Microsoft.EntityFrameworkCore;
using TaskAdminApi.Models;


namespace TaskAdminApi
{
    public class TaskAdminContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Client> Clients { get; set; }

        public TaskAdminContext(DbContextOptions<TaskAdminContext> options) : base(options)
        {
          
            //Database.EnsureDeleted();
            Database.EnsureCreated(); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {         
           modelBuilder.
                Entity<Client>()
                .HasMany(s => s.Services)
                .WithMany(c => c.Clients)
                .UsingEntity<ServicesForClient>(
                   j => j
                    .HasOne(pt => pt.Service)
                    .WithMany(t => t.ServicesForClients)
                    .HasForeignKey(pt => pt.ServiceId),
                j => j
                    .HasOne(pt => pt.Client)
                    .WithMany(p => p.ServicesForClients)
                    .HasForeignKey(pt => pt.ClientId),
                j =>
                {
                    j.Property(pt => pt.Service_Status_Complete );
                    j.Property(pt => pt.Service_Status_Pay );
                    
                    j.HasKey(t => new { t.ClientId, t.ServiceId });
                    j.ToTable("ServicesForClient");
                });

            modelBuilder.Entity<User>()
              .ToTable("Users").HasKey(p => p.Id);

        }
    }
}
