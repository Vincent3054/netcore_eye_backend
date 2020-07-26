using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite
{
    public class AnalysisLogModel
    {
        [Key,Column(TypeName = "varchar(10)"),Required]
        public string A_Id { get; set; }
        
        [ForeignKey("UserModel")]
        public string M_Id { get; set; }
        [ForeignKey("BeforeAnalysisLogModel")]
        public string B_Id { get; set; }
        [Column(TypeName = "NvarChar(Max)")]
        public string Image { get; set; }
        
        [Column(TypeName = "datetime")]
        public DateTime AnalysisTime { get; set; }

        public virtual UserModel TheUser  { get; set;}
        public virtual BeforeAnalysisLogModel TheBeforeAnalysisLogModel  { get; set;}
        public virtual ICollection<AnalysisStatusModel> AnalysisStatus { get; set; } 
    }
}