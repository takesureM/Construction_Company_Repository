using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;


namespace InstantEats.Entities
{
     public enum OrderStatus
     {

          [EnumMember(Value = "Accepted")]
          Accepted,

          [EnumMember(Value = "Order Proccessing")]
          OrderProccessing,

          [EnumMember(Value = "Order Ready")]
          OrderReady
     }
}