using AuthorizationService.Dto;
using AuthorizationService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public interface IAccounts
    {
        Task<Account> Authenticate(string email, string password);
        Task<bool> CheckNameEquality(string name);
        Task<AccountDto> CreateAccount(AccountCreateDto accountCreateDto, Roles role);
        Task<bool> DeleteAccount(Guid id);
        Task<Account> GetAccount(Guid id);
        Task<IEnumerable<AccountDto>> GetAllAccounts();
        Task<IEnumerable<AccountDto>> GetAllDeletedAccounts();
        Task<bool> RestoreAccount(Guid id);
        Task<bool> UpdateAccount(Guid id, AccountCreateDto accountCreateDto);
    }
}