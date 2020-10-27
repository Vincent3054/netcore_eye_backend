using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

namespace DBContext.EntityConfiguration
{
    public class AnalysisStatusEntityConfiguration : IEntityTypeConfiguration<AnalysisStatusModel>
    {
        public void Configure(EntityTypeBuilder<AnalysisStatusModel> builder)
        {          
            builder.HasKey(p => p.AS_ID); //主鍵

            builder.HasOne(p=>p.TheAnalysisLog)//外來鍵的表 去
                .WithMany(p=>p.AnalysisStatus)//外來鍵的表 來
                .HasForeignKey(p=>p.A_ID);//外來鍵

            builder.HasOne(p=>p.TheStatus)
                .WithMany(p=>p.AnalysisStatus)
                .HasForeignKey(p=>p.S_ID);


            builder.HasData( //Seed Data
                new AnalysisStatusModel { AS_ID = "4ccb10d5-8ee9-4110-8423-6be0856a44ed", A_ID="9e80196d-072a-4f08-87cd-1c37ccdbc814",S_ID="84d90208-6244-4b83-a714-1baebf96eaa5"}
            );
        }
    }
}