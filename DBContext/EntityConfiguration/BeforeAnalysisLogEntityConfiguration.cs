using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class BeforeAnalysisLogEntityConfiguration : IEntityTypeConfiguration<BeforeAnalysisLogModel>
{
    public void Configure(EntityTypeBuilder<BeforeAnalysisLogModel> builder)
    {
        builder.HasKey(p => p.B_Id); //主鍵
            
        Guid B_Id = Guid.NewGuid();
        builder.HasData( //Seed Data
            new BeforeAnalysisLogModel { B_Id = B_Id, RawImage="https://i.imgur.com/cfeJ9j7.png",RawTime=DateTime.Now}
        );
    }
}