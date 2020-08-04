
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class BeforeAnalysisLogModel
    {
    [Key]
    [Column(TypeName = "varchar(50)")]
    [Required]
    public string B_Id { get; set; }

    [Column(TypeName = "NvarChar(Max)")]
    public string RawImage { get; set; }
    
    [Column(TypeName = "datetime")]
    public DateTime RawTime { get; set; }
    public virtual ICollection<AnalysisLogModel> AnalysisLog { get; set; } 


    }
}