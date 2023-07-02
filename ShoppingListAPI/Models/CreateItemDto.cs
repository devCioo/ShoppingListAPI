﻿using ShoppingListAPI.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShoppingListAPI.Models
{
    public class CreateItemDto
    {
        [Required]
        [MaxLength(70)]
        public string Name { get; set; }
        public string Description { get; set; }
        public MeasureUnit MeasureUnit { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public int ShoppingListId { get; set; }
    }
}
