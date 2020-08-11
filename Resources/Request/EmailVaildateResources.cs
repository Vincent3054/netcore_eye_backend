using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Resources.Request
{

    public class EmailVaildateResources
    {
        public string Account { get; set; }
        public string AuthCode { get; set; }
    }
}

