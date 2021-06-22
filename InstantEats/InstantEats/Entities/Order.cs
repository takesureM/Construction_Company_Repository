using System;

namespace InstantEats.Entities
{
    public class Order : User
    {
        public Guid OrderId { get; set; }
        
    }
}