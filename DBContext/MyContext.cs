using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyWebsite
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
    
        public DbSet <UserModel> User { get; set; } 
        public DbSet <AnalysisLogModel> AnalysisLog{ get; set; } 
        public DbSet <StatusModel> Status { get; set; } 
        public DbSet <AnalysisStatusModel> AnalysisStatus { get; set; } 
        public DbSet <BeforeAnalysisLogModel> BeforeAnalysisLog { get; set; } 

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new AnalysisLogConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new AnalysisStatusConfiguration());

        }


    }
}