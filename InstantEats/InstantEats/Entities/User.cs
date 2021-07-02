using System;

namespace InstantEats.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}