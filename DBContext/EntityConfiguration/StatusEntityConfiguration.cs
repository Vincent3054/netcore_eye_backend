using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class StatusEntityConfiguration : IEntityTypeConfiguration<StatusModel>
{
    public void Configure(EntityTypeBuilder<StatusModel> builder)
    {
        modelBuilder.Entity<StatusModel>() //非空值
                    .IsRequired();

        builder.HasKey(p => p.S_Id); //主見

        modelBuilder.Entity<StatusModel>().HasData( //Seed Data
            new StatusModel { S_Id = 1, StatusName="坐姿警示",Message="過於前傾"}
        );
    }
}