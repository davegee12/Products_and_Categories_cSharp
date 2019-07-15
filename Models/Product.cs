using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace ProductsAndCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId{get;set;}

        // Product Name
        [Required(ErrorMessage="Name is required")]
        [MinLength(2, ErrorMessage="Name must be longer than two characters")]
        public string ProductName{get;set;}

        // Description
        [Required(ErrorMessage="Description is required")]
        [MinLength(2, ErrorMessage="Description must be longer than two characters")]
        public string Description{get;set;}

        // Price
        [Required(ErrorMessage="Price is required")]
        public double Price{get;set;}
        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdateAt{get;set;} = DateTime.Now;

        public List<Association> Associations{get;set;}
    }
}