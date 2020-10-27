using System.Collections.Generic;
using DBContext.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DBContext
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }
    
        public DbSet <MemberModel> Member { get; set; } 
        public DbSet <AnalysisLogModel> AnalysisLog{ get; set; } 
        public DbSet <StatusModel> Status { get; set; } 
        public DbSet <AnalysisStatusModel> AnalysisStatus { get; set; } 

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MemberEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AnalysisLogEntityConfiguration());
            modelBuilder.ApplyConfiguration(new StatusEntityConfiguration());
            modelBuilder.ApplyConfiguration(new AnalysisStatusEntityConfiguration());
        }   
    }
}