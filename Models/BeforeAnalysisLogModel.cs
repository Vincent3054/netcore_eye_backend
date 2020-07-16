
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite
{
    public class BeforeAnalysisLogModel
    {
    [Key]
    [Column(TypeName = "uniqueidentifier")]
    public Guid B_Id { get; set; }
    [Column(TypeName = "NvarChar(Max)")]
    public string RawImage { get; set; }
    [Column(TypeName = "DateTime")]
    public DateTime RawTime { get; set; }
    }
}