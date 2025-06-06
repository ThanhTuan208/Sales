﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CRUD_asp.netMVC.Models.Product
{
    public class Brand
    {
        [Key]
        public int ID { get; set; }

        [Required, StringLength(50)]
        public string? Name { get; set; }

        [Required, Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [Required]
        public string? PicturePath { get; set; }

        public List<Products>? products { get; set; }
    }
}
