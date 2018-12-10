using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsAndCategories.Models
{
    public class Category
    {
        [Key]
        public int CategoryId {get;set;}

        [Required]
        [MinLength(3, ErrorMessage = "Name must be 3 characters or longer.")]
        [Display(Name = "Category:")]
        public string Name {get;set;}
        public List<Association> ProductsInCategory {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}