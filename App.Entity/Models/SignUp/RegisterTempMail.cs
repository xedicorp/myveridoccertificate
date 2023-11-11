using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Entity.Models.SignUp
{
    public class RegisterTempMail
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Hash { get; set; }
        public bool IsCompleted { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
    }
}
