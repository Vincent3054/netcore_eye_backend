using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;
namespace DBContext.EntityConfiguration
{
    public class StatusEntityConfiguration : IEntityTypeConfiguration<StatusModel>
    {
        public void Configure(EntityTypeBuilder<StatusModel> builder)
        {
            builder.HasKey(p => p.S_ID); //主鍵
        
            builder.HasData( //Seed Data
                new StatusModel { S_ID = "84d90208-6244-4b83-a714-1baebf96eaa5", StatusName="坐姿警示",StatusMessage="坐姿過於前傾"}
            );
        }
    }
}