﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entity.Models.Mail
{
    public class FreeTrialReminder
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ExpireDate { get; set; }
    }
}
