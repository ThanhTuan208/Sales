﻿using CRUD_asp.netMVC.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Account
{
    public class Manager : FieldGeneral
    {
        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
    }
}
