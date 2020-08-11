using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
namespace DBContext.EntityConfiguration
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(p => p.M_Id); //主鍵
            builder.HasData( //Seed Data
                new UserModel { M_Id = "1", Email = "ok96305@gmail.com",Account="admin001", Password = "12345", Name = "陳建成",Sex="男",BirthDate=DateTime.Now, Role = true, CreateTime=DateTime.Now,AuthCode=""}
            );

        }
    }
}