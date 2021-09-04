using AuthorizationService.Dto;
using AuthorizationService.Extensions;
using AuthorizationService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public class AccountsInSQlRepository : IAccounts, IDisposable
    {
        private readonly AuthorizationDbContext _db;
        private readonly ILogger<AccountsInSQlRepository> _logger;

        public AccountsInSQlRepository(AuthorizationDbContext db,
            ILogger<AccountsInSQlRepository> logger)
        {
            _db = db;
            _logger = logger;
            _logger.LogTrace($"using AccountsInSQlRepositoryConstructor");
            CreateSuperAdmin();
        }


        private void CreateSuperAdmin()
        {
            _logger.LogTrace($"using {nameof(CreateSuperAdmin)}");

            if (!_db.Accounts.Any())
            {
                _logger.Warn("SuperAdmin has been created by default");

                string password = "123";
                var salt = GenerateSalt();

                var superLogin = new Login()
                {
                    Email = "superAdminAndrewst@bk.ru",
                    Salt = Convert.ToBase64String(salt),
                    PasswordHash = Convert.ToBase64String(password.ToPasswordHash(salt)),
                };

                var account = new Account
                {
                    NickName = "superAdminAndrewst",
                    Role = Roles.superadministrator,
                    Login = superLogin
                };

                _db.Accounts.Add(account);
                _db.SaveChanges();
            }
            else
            {
                _logger.Info("SuperAdmin has NOT been created");
            }
        }


        public async Task<IEnumerable<AccountDto>> GetAllAccounts()
        {
            await Task.CompletedTask;
            _logger.LogTrace($"using {nameof(GetAllAccounts)}");

            return _db.Accounts
                .Where(c => c.IsDeleted == false)
                .Select(c => new AccountDto(c));
        }


        public async Task<Account> GetAccount(Guid id)
        {
            _logger.LogTrace($"using {nameof(GetAccount)}");

            var account = await _db.Accounts.FirstOrDefaultAsync(c => c.EntityId == id && c.IsDeleted == false);

            if (account == null) return null;

            return account;
        }

        public async Task<bool> CheckNameEquality(string name)
        {
            _logger.LogTrace($"using {nameof(CheckNameEquality)}");
            var existAccounts = await GetAllAccounts();
            var existAccount = existAccounts.FirstOrDefault(a => a.NickName == name);

            if (existAccount != null) return true;

            return false;
        }

        public async Task<AccountDto> CreateAccount(AccountCreateDto accountCreateDto, Roles role)
        {
            var salt = GenerateSalt();
            var enteredPassHash = accountCreateDto.Password.ToPasswordHash(salt);

            Login newLoginModel = new Login()
            {
                Email = accountCreateDto.Email,
                Salt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(enteredPassHash),
            };

            Account account = new Account()
            {
                Login = newLoginModel,
                NickName = accountCreateDto.NickName,
                Role = role
            };

            await _db.Accounts.AddAsync(account);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return new AccountDto(account);

        }


        public async Task<bool> UpdateAccount(Guid id, AccountCreateDto accountCreateDto)
        {
            _logger.LogTrace($"using {nameof(UpdateAccount)}");

            var account = await _db.Accounts.Include(a => a.Login)
                                           .FirstOrDefaultAsync(c => c.EntityId == id);

            if (account == null) return false;

            var salt = GenerateSalt();
            var enteredPassHash = accountCreateDto.Password.ToPasswordHash(salt);

            account.NickName = accountCreateDto.NickName;
            account.Login.Email = accountCreateDto.Email;
            account.Login.Salt = Convert.ToBase64String(salt);
            account.Login.PasswordHash = Convert.ToBase64String(enteredPassHash);

            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }


        public async Task<bool> DeleteAccount(Guid id)
        {
            _logger.LogTrace($"using {nameof(DeleteAccount)}");

            var account = await _db.Accounts.FirstOrDefaultAsync(c => c.EntityId == id);

            if (account == null) return false;

            account.IsDeleted = true;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public async Task<IEnumerable<AccountDto>> GetAllDeletedAccounts()
        {
            await Task.CompletedTask;
            _logger.LogTrace($"using {nameof(GetAllDeletedAccounts)}");

            return _db.Accounts
                .Where(c => c.IsDeleted == true)
                .Select(c => new AccountDto(c));
        }

        public async Task<bool> RestoreAccount(Guid id)
        {
            _logger.LogTrace($"using {nameof(RestoreAccount)}");

            var account = await _db.Accounts.FirstOrDefaultAsync(l => l.EntityId == id);

            if (account == null) return false;

            account.IsDeleted = false;

            await _db.SaveChangesAsync();
            await _db.DisposeAsync();

            return true;
        }

        public static byte[] GenerateSalt()
        {
            using var randomNumberGenerator = new RNGCryptoServiceProvider();
            var randomNumber = new byte[16];
            randomNumberGenerator.GetBytes(randomNumber);

            return randomNumber;
        }

        public async Task<Account> Authenticate(string email, string password)
        {
            _logger.LogTrace($"using {nameof(Authenticate)}");

            var login = await _db.Logins.FirstOrDefaultAsync(c => c.Email == email);

            var enteredPassHash = password.ToPasswordHash(Convert.FromBase64String(login.Salt));

            var isValid = Convert.ToBase64String(enteredPassHash) == login.PasswordHash;

            var account = await GetAccount(login.AccountId);

            return isValid ? account : null;

        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
