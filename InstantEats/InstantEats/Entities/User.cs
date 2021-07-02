﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstantEats.Entities
{
    public class User
    {
        public Guid UserId {get; set;}
        public bool IsDeleted { get; set; }
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public User()
        {
            IsDeleted = false;
        }
    }
}
