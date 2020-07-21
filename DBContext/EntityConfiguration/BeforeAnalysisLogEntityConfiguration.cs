using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class BeforeAnalysisLogEntityConfiguration : IEntityTypeConfiguration<BeforeAnalysisLogModel>
{
    public void Configure(EntityTypeBuilder<BeforeAnalysisLogModel> builder)
    {
        modelBuilder.Entity<BeforeAnalysisLogModel>() //非空值
                    .IsRequired();

        builder.HasKey(p => p.B_Id); //主見

        modelBuilder.Entity<BeforeAnalysisLogModel>().HasData( //Seed Data
            new BeforeAnalysisLogModel { B_Id = 1, RawImage="https://i.imgur.com/cfeJ9j7.png",RawTime="2020-07-21T19:15:40"}
        );
    }
}