using System;

namespace InstantEats.Entities
{
    public class Order : User
    {
        public Guid OrderId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
        public OrderStatus Status { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        
    }
}