using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class StatusConfiguration : IEntityTypeConfiguration<StatusModel>
{
    public void Configure(EntityTypeBuilder<StatusModel> builder)
    {
	// builder.HasKey(o => o.OrderNumber);
	// builder.Property(t => t.OrderDate);
    }
}