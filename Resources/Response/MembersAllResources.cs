using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Resources.Response{

    public class MembersAllResources
    {
       
        public string Account { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime BirthDate { get; set; }
        
        
    }
}

