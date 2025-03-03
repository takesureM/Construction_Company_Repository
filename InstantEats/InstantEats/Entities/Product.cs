﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstantEats.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }
        
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
