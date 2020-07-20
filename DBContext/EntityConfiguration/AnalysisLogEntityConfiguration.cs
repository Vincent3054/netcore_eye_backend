using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class AnalysisLogEntityConfiguration : IEntityTypeConfiguration<AnalysisLogModel>
{
    public void Configure(EntityTypeBuilder<AnalysisLogModel> builder)
    {
        builder.HasKey(p => p.A_Id);//主見

        builder.HasOne(p=>p.TheUser)//外來鍵的表 去
               .WithMany(p=>p.AnalysisLog)//外來鍵的表 來
               .HasForeignKey(p=>p.M_Id);//外來鍵

        builder.HasOne(p=>p.TheBeforeAnalysisLogModel)//外來鍵的表 去
               .WithMany(p=>p.AnalysisLog)//外來鍵的表 來
               .HasForeignKey(p=>p.B_Id);//外來鍵
                      
               
    }
}