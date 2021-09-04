using AuthorizationService.Dto;
using AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public interface IRefreshTokens
    {
        Func<DateTime> GetDtFunc { get; set; }

        Task<RefreshToken> CreateRefreshToken(Account account, int expiresSec);
        Task<bool> DeleteRefreshToken(Guid id);
        Task<bool> DeleteRefreshTokensForAccount(Guid accountId);
        Task<IEnumerable<RefreshTokenDto>> GetAllRefreshTokens();
        Task<IEnumerable<RefreshTokenDto>> GetAllRefreshTokens(Guid accountId);
        Task<RefreshToken> ReCreateRefreshToken(Guid previousRefreshId, int expiresSec);
    }
}