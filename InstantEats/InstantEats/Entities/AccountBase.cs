using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstantEats.Entities
{
    public class AccountBase
    {
        public Guid AccountId {get; set;}
        public bool IsDeleted { get; set; }

    }
}
