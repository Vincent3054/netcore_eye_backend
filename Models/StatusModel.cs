using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite
{
    public class StatusModel
    {
        [Key] 
        [Column(TypeName = "varchar(100)")]
        public string S_Id { get; set; }
        [Column(TypeName = "nvarChar(36)")]
        public string StatusName { get; set; }
        [Column(TypeName = "Nvarchar(200)")]
        public string Message { get; set; }
        
        public virtual ICollection<AnalysisStatusModel> AnalysisStatus { get; set; } 
    }
}