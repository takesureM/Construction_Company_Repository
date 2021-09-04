using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Entity
{
    public abstract class EntityBase
    {
        public Guid EntityId { get; set; }
        public bool IsDeleted { get; set; }

        public EntityBase()
        {
            IsDeleted = false;
        }
    }
}
