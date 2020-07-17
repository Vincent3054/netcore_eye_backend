using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class AnalysisLogConfiguration : IEntityTypeConfiguration<AnalysisLogModel>
{
    public void Configure(EntityTypeBuilder<AnalysisLogModel> builder)
    {
	// builder.HasKey(o => o.OrderNumber);
	// builder.Property(t => t.OrderDate);
    }
}