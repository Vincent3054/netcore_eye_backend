using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class AnalysisLogModel
    {   
        //分析資料編號(主鍵)
        [Key]
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string A_ID { get; set; }
        
        //會員編號(外來鍵)
        [ForeignKey("MemberModel")]
        public string M_ID { get; set; }

        //圖片檔名
        [Column(TypeName = "nvarChar(30)")]
        public string Image_Name { get; set; }
        
        //圖片路徑
        [Column(TypeName = "varChar(Max)")]
        public string Image_Location { get; set; }
        
        //建立時間
        [Column(TypeName = "datetime")]
        public DateTime UpdateTime { get; set; }

        public virtual MemberModel TheMember { get; set;}
        public virtual ICollection<AnalysisStatusModel> AnalysisStatus { get; set; } 
    }
}