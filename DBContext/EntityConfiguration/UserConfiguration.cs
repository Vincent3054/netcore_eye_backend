using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class UserConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
	// builder.HasKey(o => o.OrderNumber);
	// builder.Property(t => t.OrderDate);
    }
}