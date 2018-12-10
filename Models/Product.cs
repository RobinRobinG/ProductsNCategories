using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsAndCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get;set;}

        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3 characters or longer.")]
        [Display(Name = "Product Name:")]
        public string Name {get;set;}
        
        [Required]
        [MinLength(10, ErrorMessage = "Description must be 10 characters or longer.")]
        [Display(Name = "Description:")]
        public string Description {get;set;}

        [Required]
        [Display(Name = "Price:")]
        public int Price {get;set;}
        public List<Association> ProductCategories {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;

    }
}