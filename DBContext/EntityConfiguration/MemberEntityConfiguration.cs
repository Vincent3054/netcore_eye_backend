using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
namespace DBContext.EntityConfiguration
{
    public class MemberEntityConfiguration : IEntityTypeConfiguration<MemberModel>
    {
        public void Configure(EntityTypeBuilder<MemberModel> builder)
        {
            builder.HasKey(p => p.M_ID); //主鍵
            builder.HasData( //Seed Data
                new MemberModel
                {
                    M_ID = "9dc1dfd0-041c-4f7c-9e9d-efe7afb5ecfd",
                    Account = "admin001",
                    Password = "12345",
                    Email = "ok96305@gmail.com",
                    Name = "陳建成",
                    Sex = "男",
                    BirthDate = DateTime.Now,
                    Role = true,
                    CreateTime = DateTime.Now,
                    AuthCode = ""
                }
            );

        }
    }
}