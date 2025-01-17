﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ProductSoftwareAPI.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; } //quantity
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public ProductCategory ProductCategory { get; set; }
    }
}
