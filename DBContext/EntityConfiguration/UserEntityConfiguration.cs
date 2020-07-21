using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        modelBuilder.Entity<UserModel>() //非空值
                    .IsRequired();

        builder.HasKey(p => p.M_Id); //主見

        modelBuilder.Entity<UserModel>().HasData( //Seed Data
            new UserModel { M_Id = 1, Email = "ok96305@gmail.com", Passsword = "123456789", Name = "陳建成", Role = 1, CreateTime = "2020-07-21T19:48:30" }
        );

    }
}