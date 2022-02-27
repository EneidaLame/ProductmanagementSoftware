﻿using ProductSoftwareAPI.Entities;
using ShopOnline.Models.Dtos;

namespace ProductSoftwareAPI.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products,
            IEnumerable<ProductCategory> productCategories)
        { 
            return (from product in products
                    join productCategory in productCategories
                    on product.CategoryId equals productCategory.Id
                    select new ProductDto 
                    { 
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        ImageUrl = product.ImageUrl,
                        Price = product.Price,
                        Qty = product.Qty,
                        CategoryId = productCategory.Id,
                        CategoryName = productCategory.Name
                    }).ToList();
        }
    }
}