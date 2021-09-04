using AuthorizationService.Certificates;
using AuthorizationService.Dto;
using AuthorizationService.Extensions;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
    /// <response code="200">Операция проведена успешно</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [Route("identity/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthorizationController : ControllerBase
    {
        public Func<DateTime> GetCurrentDtFunc = () => DateTime.Now;
        private readonly IConfiguration _config;
        private readonly IAccounts _accounts;
        private readonly IRefreshTokens _refreshTokens;

        public AuthorizationController(IAccounts accounts,
            IConfiguration config,
            IRefreshTokens refreshTokens)
        {
            _refreshTokens = refreshTokens;
            _accounts = accounts;
            _config = config;
        }

        /// <summary>
        /// Создание JWT
        /// </summary>
        /// <response code="401">Не верные логин/пароль</response>
        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TokenDto>> CreateToken([FromBody] SignIn signIn)
        {
            var account = await _accounts.Authenticate(signIn.Email, signIn.Password);

            if (account == null) return Unauthorized();

            var expiresSec = int.Parse(_config["Jwt:ExpiresSec"]);

            var refresh = await _refreshTokens.CreateRefreshToken(account, 864000); 

            var token = await BuildToken(new AccountDtoForAuthorization(account), refresh.RefreshTokenId, expiresSec);

            return Ok(token);
        }

        /// <summary>
        /// Обновление JWT
        /// </summary>
        /// <response code="401">Токен просрочен. Вход по логину и паролю (/api/Token/signin)</response>
        /// <response code="403">Аккаунт деактивирован</response>
        [HttpPost("refreshId={id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<TokenDto>> RefreshToken(Guid id)
        {
            var expiresSec = int.Parse(_config["Jwt:ExpiresSec"]);
            var newRefreshToken = await _refreshTokens.ReCreateRefreshToken(id, 864000); //TODO В конфиг
            if (newRefreshToken == null) return Unauthorized();

            var account = await _accounts.GetAccount(newRefreshToken.AccountId);
            if (account == null) return Forbid();

            var token = await BuildToken(new AccountDtoForAuthorization(account), newRefreshToken.RefreshTokenId, expiresSec);
            return Ok(token);
        }

        /// <summary>
        /// Получить список всех RefreshToken 
        /// </summary>
        /// <returns></returns>
        /// <response code = "204"> Список RefreshToken пуст</response>
        /// <response code="401">Доступ только для администратора</response>
        [HttpGet]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RefreshToken[]>> GetAll()
        {
            var tokens = await _refreshTokens.GetAllRefreshTokens();

            return Ok(tokens);
        }

        /// <summary>
        /// Получить список всех RefreshToken для аккаунта
        /// </summary>
        /// <returns></returns>
        /// <response code="204">Список RefreshToken пуст</response>
        /// <response code="401">Доступ только для администратора</response>
        [HttpGet("accountId={accountId}")]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<RefreshToken[]>> GetAll(Guid accountId)
        {
            var tokens = await _refreshTokens.GetAllRefreshTokens(accountId);

            return Ok(tokens);
        }

        /// <summary>
        /// Удалить RefreshToken 
        /// </summary>
        /// <returns></returns>
        /// <response code="401">Доступ только для администратора</response>
        [HttpDelete("tokenId={tokenId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator)]
        public async Task<ActionResult<RefreshToken[]>> DeleteToken(Guid tokenId)
        {
            bool isDeleted = await _refreshTokens.DeleteRefreshToken(tokenId);
            return isDeleted ? Ok() : NotFound();
        }

        /// <summary>
        /// Удалить RefreshToken для аккаунта
        /// </summary>
        /// <returns></returns>
        /// <response code="401">Доступ только для администратора</response>
        [HttpDelete("accountId={accountId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator)]
        public async Task<ActionResult> DeleteTokensForAccount(Guid accountId)
        {
            bool isDeleted = await _refreshTokens.DeleteRefreshTokensForAccount(accountId);
            return isDeleted ? Ok() : NotFound();
        }

        private async Task<TokenDto> BuildToken(AccountDtoForAuthorization account, Guid refreshId, int expiresSec)
        {
            var expiresDt = GetCurrentDtFunc.Invoke().AddSeconds(expiresSec);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, account.NickName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role),
                new Claim(ClaimTypes.PrimarySid, account.Id.ToString()),
            };

            SigningAudienceCertificate signingAudienceCertificate = new SigningAudienceCertificate(_config);
            var creds = await signingAudienceCertificate.GetAudienceSigningKey();

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: expiresDt,
                signingCredentials: creds);



            return new TokenDto()
            {
                Expires = expiresDt,
                Jwt = new JwtSecurityTokenHandler().WriteToken(token),
                Account = account,
                RefreshTokenId = refreshId
            };
        }
    }
}
