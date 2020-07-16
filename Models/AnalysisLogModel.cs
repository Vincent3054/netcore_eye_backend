using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite
{
    public class AnalysisLogModel
    {
        [Key]
        [Column(TypeName = "uniqueidentifier")]
        public Guid A_Id { get; set; }
        
        [ForeignKey("UserModel")]
        public string M_Id { get; set; }
        
        [Column(TypeName = "NvarChar(Max)")]
        public string Image { get; set; }
        
        [Column(TypeName = "DateTime")]
        public DateTime AnalysisTime { get; set; }

        public virtual UserModel TheUser  { get; set;}
        
        public virtual ICollection<AnalysisStatusModel> AnalysisStatus { get; set; } 
    }
}