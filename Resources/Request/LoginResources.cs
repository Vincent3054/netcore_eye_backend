using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Resources.Request{

    public class LoginResources
    {
        public string Account { get; set; }
        public string Password { get; set; }  
        
    }
}

