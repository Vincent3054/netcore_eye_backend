using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class BeforeAnalysisLogConfiguration : IEntityTypeConfiguration<BeforeAnalysisLogModel>
{
    public void Configure(EntityTypeBuilder<BeforeAnalysisLogModel> builder)
    {
	// builder.HasKey(o => o.OrderNumber);
	// builder.Property(t => t.OrderDate);
    }
}