using AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Dto
{
    public class AccountDtoForAuthorization
    {
        public AccountDtoForAuthorization(Account account)
        {
            Id = account.EntityId;
            NickName = account.NickName;
            Role = account.Role.ToString(); 
        }

        public Guid Id { get; set; }
        public string NickName { get; set; }
        public string Role { get; set; }
    }
}
