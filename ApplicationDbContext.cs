using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Student_manage.Models;

namespace Student_manage.Models
{
   
    
        // Constructor connects to the DB using the connection string
     

    public class ApplicationDbContext : DbContext
    { 
        public ApplicationDbContext(): base("MyDbConnection") { }
        public DbSet<Admins> Admins { get; set; } // for Admin table

        public DbSet<Users> Users { get; set; }//  for User Table

        public DbSet <StudentRegister> student_register { get; set; }// for Registeration 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentRegister>().ToTable("student_register");
        }
        public DbSet<State_Master> state_master { get; set; }

        public DbSet<City_Master> city_master { get; set; }

        public DbSet<FeesStructure> fees_structure { get; set; }
        public DbSet<MasterData> masterdata { get; set; }

    }
}
