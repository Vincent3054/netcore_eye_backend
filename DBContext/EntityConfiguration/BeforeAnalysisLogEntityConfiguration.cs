using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class BeforeAnalysisLogEntityConfiguration : IEntityTypeConfiguration<BeforeAnalysisLogModel>
{
    public void Configure(EntityTypeBuilder<BeforeAnalysisLogModel> builder)
    {
        builder.HasKey(p => p.B_Id); //主見

    }
}