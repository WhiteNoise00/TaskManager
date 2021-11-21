using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskAdminApi.Models;


namespace TaskAdminApi
{
    public class TaskAdminContext : DbContext
    {
        public DbSet<Service> Services { get; set; }
        public DbSet<Client> Clients { get; set; }


        public TaskAdminContext(DbContextOptions<TaskAdminContext> options) : base(options)
        {
           // Database.EnsureDeleted();
            Database.EnsureCreated();

        }
    }
}
