
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class AnalysisStatusModel
    {
    [Key]
    [Column(TypeName = "int")]
    [Required]
    public int AS_Id { get; set; }
    
    [ForeignKey("AnalysisLogModel")]
    public string A_Id { get; set; }
    public virtual AnalysisLogModel TheAnalysisLog  { get; set;}

    [ForeignKey("StatusModel")]
    public string S_Id { get; set; }
    public virtual StatusModel TheStatus  { get; set;}
    }
}