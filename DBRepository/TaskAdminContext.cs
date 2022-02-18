using Microsoft.EntityFrameworkCore;
using TaskAdminApi.Models;


namespace TaskAdminApi
{
    public class TaskAdminContext : DbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<Client> Clients { get; set; }

        public TaskAdminContext(DbContextOptions<TaskAdminContext> options) : base(options)
       {
            //наиболее приемлимый вариант реализации репозитория обозначен в статье от майкрософ
        //засовываю строку подключения и конфигурацию Buildera (Code First) Startup.cs
        //Но тогда и Context придется спрятать в проект web- приложения, ведь CodeFirst - штука только для ASP Net
           //Database.EnsureDeleted();
          //или понять, что я вообще хочу: сделать универсальный класс для люого типа приложения, или же для любого типа подключения
           Database.EnsureCreated(); //придется убрать эту строку, так как я буду подключаться к базе данных через строку
           //подключения, то бишь, мне еще придется создать миграцию
           //Неверно. Придется оставить данную команду, т.к. при отсутствии БД она создаст ее по модели

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

        }
    }
}
