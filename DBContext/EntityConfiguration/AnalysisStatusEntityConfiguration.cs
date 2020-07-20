using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyWebsite;

public class AnalysisStatusEntityConfiguration : IEntityTypeConfiguration<AnalysisStatusModel>
{
    public void Configure(EntityTypeBuilder<AnalysisStatusModel> builder)
    {
        
        builder.HasKey(p => p.AS_Id); //主見


        builder.HasOne(p=>p.TheAnalysisLog)//外來鍵的表 去
               .WithMany(p=>p.AnalysisStatus)//外來鍵的表 來
               .HasForeignKey(p=>p.A_Id);//外來鍵

        builder.HasOne(p=>p.TheStatus)
               .WithMany(p=>p.AnalysisStatus)
               .HasForeignKey(p=>p.S_Id);

    }
}