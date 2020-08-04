using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
namespace DBContext.EntityConfiguration
{
    public class BeforeAnalysisLogEntityConfiguration : IEntityTypeConfiguration<BeforeAnalysisLogModel>
    {
        public void Configure(EntityTypeBuilder<BeforeAnalysisLogModel> builder)
        {
            builder.HasKey(p => p.B_Id); //主鍵
            builder.HasData( //Seed Data
                new BeforeAnalysisLogModel { B_Id = "1", RawImage="https://i.imgur.com/cfeJ9j7.png",RawTime= DateTime.Now}
            );
        }
    }
}
