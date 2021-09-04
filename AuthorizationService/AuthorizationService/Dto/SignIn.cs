

using AspNetCoreValidationLibrary;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Dto
{
    public class SignIn
    {
        /// <summary>
        /// почта
        /// </summary>
        [Required]
        [Length(MinLen = 1, MaxLen = 100, ErrMes = "must be in range")]
        public string Email { get; set; }

        /// <summary>
        /// пароль
        /// </summary>
        [Required]
        [Length(MinLen = 1, MaxLen = 100, ErrMes = "must be in range")]
        public string Password { get; set; }
    }

}
