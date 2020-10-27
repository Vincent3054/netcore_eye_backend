using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class StatusModel
    {
        //警示狀態編號(主鍵)
        [Key] 
        [Column(TypeName = "varchar(50)")]
        [Required]
        public string S_ID { get; set; }

        //警示狀態名稱
        [Column(TypeName = "nvarChar(40)")]
        public string StatusName { get; set; }

        //警示訊息
        [Column(TypeName = "nvarchar(200)")]
        public string StatusMessage { get; set; }
        
        public virtual ICollection<AnalysisStatusModel> AnalysisStatus { get; set; } 
    }
}