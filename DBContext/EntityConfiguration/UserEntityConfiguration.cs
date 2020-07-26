using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserModel>
{
    public void Configure(EntityTypeBuilder<UserModel> builder)
    {
        builder.HasKey(p => p.M_Id); //主鍵
        builder.HasData( //Seed Data
            new UserModel { M_Id = "1", Email = "ok96305@gmail.com", Passsword = "12345", Name = "陳建成", Role = true, CreateTime=DateTime.Now}
        );

    }
}