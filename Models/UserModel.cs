using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite
{
    public class UserModel
    {   
        //會員編號
        [Key]
        [Column(TypeName = "varchar(10)")]
        [Required]
        public string M_Id { get; set; }
        //帳號
        [Column(TypeName = "varchar(30)")]
        public string Account { get; set; }
        //密碼
        [Column(TypeName = "varchar(100)")]
        public string Password { get; set; }
        //信箱
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        //姓名
        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; }
        //性別
        [Column(TypeName = "varchar(2)")]
        public string Sex { get; set; }
        //生日
        [Column(TypeName = "datetime")]
        public DateTime BirthDate { get; set; }
        //權限
        [Column(TypeName = "bit")]
        public bool  Role { get; set; }
        //建立日期
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public virtual ICollection<AnalysisLogModel> AnalysisLog { get; set; }
        
    }
}