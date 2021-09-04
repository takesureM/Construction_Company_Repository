using System;

namespace InstantEats.Entities
{
    public class Order : AccountBase
    {
        public Guid OrderId { get; set; }
        
    }
}