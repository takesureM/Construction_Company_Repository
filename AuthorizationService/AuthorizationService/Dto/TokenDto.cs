using AuthorizationService.Models;
using System;

namespace AuthorizationService.Dto
{
    public class TokenDto
    {
        public AccountDtoForAuthorization Account { get; set; }
        public string Jwt { get; set; }
        public DateTime Expires { get; set; }
        public Guid RefreshTokenId { get; set; }
    }
}
