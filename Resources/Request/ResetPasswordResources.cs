using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Resources.Request
{

    public class ResetPasswordResources
    {
        public string Account { get; set; }
        public string AuthCode { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordCheck { get; set; }
    }
}

