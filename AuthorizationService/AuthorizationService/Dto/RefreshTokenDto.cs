using AuthorizationService.Models;
using System;

namespace AuthorizationService.Dto
{
    public class RefreshTokenDto
    {
        public RefreshTokenDto(RefreshToken refreshToken)
        {
            RefreshTokenId = refreshToken.RefreshTokenId;
            CreateDate = refreshToken.CreateDate;
            ExpiresDate = refreshToken.ExpiresDate;
            AccountId = refreshToken.AccountId;
        }
        public Guid RefreshTokenId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiresDate { get; set; }
        public Guid AccountId { get; set; }
    }
}
