using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
namespace DBContext.EntityConfiguration
{
    public class StatusEntityConfiguration : IEntityTypeConfiguration<StatusModel>
    {
        public void Configure(EntityTypeBuilder<StatusModel> builder)
        {
            builder.HasKey(p => p.S_Id); //主鍵
        
            builder.HasData( //Seed Data
                new StatusModel { S_Id = "1", StatusName="坐姿警示",Message="坐姿過於前傾"}
            );
        }
    }
}