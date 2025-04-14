﻿using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account
{
    public abstract class FieldGeneral
    {
        [Key] public int ID { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? FullName { get; set; }
        
        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? UserName { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? Password { get; set; }

        [Required, Phone]
        public string? PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? Address { get; set; }

        [Required, StringLength(50, ErrorMessage = "Loi {0}")]
        public string? Position { get; set; } = "Customer";

        public int? UserID { get; set; }
        public Users? Users { get; set; }
    }
}
