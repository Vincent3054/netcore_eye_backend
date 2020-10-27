using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;


namespace DBContext.EntityConfiguration
{
    public class AnalysisLogEntityConfiguration : IEntityTypeConfiguration<AnalysisLogModel>
    {
        public void Configure(EntityTypeBuilder<AnalysisLogModel> builder)
        {
            builder.HasKey(p => p.A_ID);//主鍵

            builder.HasOne(p => p.TheMember)//外來鍵的表 去
                .WithMany(p => p.AnalysisLog)//外來鍵的表 來
                .HasForeignKey(p => p.M_ID);//外來鍵

            builder.HasData( //Seed Data
                new AnalysisLogModel
                {
                    A_ID = "9e80196d-072a-4f08-87cd-1c37ccdbc814",
                    M_ID = "9dc1dfd0-041c-4f7c-9e9d-efe7afb5ecfd",
                    Image_Name = "分析紀錄_" + DateTime.Now,
                    Image_Location="https://i.imgur.com/UL8Jk6A.png",
                    UpdateTime = DateTime.Now
                }
            );
        
        }
    }
}