using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class StatusEntityConfiguration : IEntityTypeConfiguration<StatusModel>
{
    public void Configure(EntityTypeBuilder<StatusModel> builder)
    {
        builder.HasKey(p => p.S_Id); //主見

    }
}