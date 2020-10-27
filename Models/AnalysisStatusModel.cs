
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class AnalysisStatusModel
    {
        //流水號(主鍵)
        [Key]
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string AS_ID { get; set; }
        
        //分析資料編號(外來鍵)
        [ForeignKey("AnalysisLogModel")]
        public string A_ID { get; set; }

        //警示狀態編號(外來鍵)
        [ForeignKey("StatusModel")]
        public string S_ID { get; set; }

        public virtual AnalysisLogModel TheAnalysisLog  { get; set;}
        public virtual StatusModel TheStatus  { get; set;}
    }
}