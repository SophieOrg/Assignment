using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using FMS.Data.Models;

namespace FMS.Data.Repository
{

    public class DataContext : DbContext
    {  
        public DbSet<Dog> Dogs { get; set; }
        public DbSet<MedicalHistory> MedicalHistorys { get; set; }

        public DbSet<AdoptionApplication> AdoptionApplications { get; set; }
        public DbSet<User> Users { get; set; }
               
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
            .UseSqlite("Filename=data.db");
            
        }

        // custom method used in development to keep database in sync with models
        //must make sure the database exists
        public void Initialise() 
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
    }
} 
