using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Resources{

    public class UserResources
    {
        public string M_Id { get; set; }
        public string Email { get; set; }
        public string Passsword { get; set; }
        public string Name { get; set; }//格式
        
        
    }
}

