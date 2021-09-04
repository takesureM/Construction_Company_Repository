using AuthorizationService.Models;

namespace AuthorizationService.Dto
{
    public class AccountDto
    {
        public AccountDto(Account account)
        {
            NickName = account.NickName;
            Role = account.Role.ToString();
        }

        public string NickName { get; set; }
        public string Role { get; set; }

    }
}