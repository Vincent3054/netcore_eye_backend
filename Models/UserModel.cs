using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite
{
    public class UserModel
    {
        [Key]
        [Column(TypeName = "varchar(10)")]
        [Required]
        public string M_Id { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string Account { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Password { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; }
        [Column(TypeName = "bit")]
        public bool  Role { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        
        public virtual ICollection<AnalysisLogModel> AnalysisLog { get; set; } 
    }
}