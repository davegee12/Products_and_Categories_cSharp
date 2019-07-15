using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;


namespace ProductsAndCategories.Models
{
    public class Association
    {
        [Key]
        public int AssociationId{get;set;}

        public int ProductId{get;set;}

        public int CategoryId{get;set;}

        public DateTime CreatedAt{get;set;} = DateTime.Now;
        public DateTime UpdateAt{get;set;} = DateTime.Now;

        public Product AssociationProduct{get;set;}
        public Category AssociationCategory{get;set;}
    }
}