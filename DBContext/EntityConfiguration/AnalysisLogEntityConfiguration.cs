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
            builder.HasKey(p => p.A_Id);//主鍵

            builder.HasOne(p=>p.TheUser)//外來鍵的表 去
                .WithMany(p=>p.AnalysisLog)//外來鍵的表 來
                .HasForeignKey(p=>p.M_Id);//外來鍵

            builder.HasOne(p=>p.TheBeforeAnalysisLogModel)//外來鍵的表 去
                .WithMany(p=>p.AnalysisLog)//外來鍵的表 來
                .HasForeignKey(p=>p.B_Id);//外來鍵
            
            builder.HasData( //Seed Data
                new AnalysisLogModel { A_Id = "1", M_Id="1",B_Id="1",Image = "https://i.imgur.com/PuC21Ma.png",AnalysisTime= DateTime.Now}
            );
                
        }
    }
}