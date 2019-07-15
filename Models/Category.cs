using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace ProductsAndCategories.Models
{
    public class Category
    {
        [Key]
        public int CategoryId{get;set;}

        // Category Name
        [Required(ErrorMessage="Name is required")]
        [MinLength(2, ErrorMessage="Name must be longer than two characters")]
        public string CategoryName{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdateAt{get;set;} = DateTime.Now;

        public List<Association> Associations{get;set;}
    }
}