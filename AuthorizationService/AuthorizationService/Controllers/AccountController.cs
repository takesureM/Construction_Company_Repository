using AuthorizationService.Dto;
using AuthorizationService.Extensions;
using AuthorizationService.Models;
using AuthorizationService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace AuthorizationService.Controllers
{
    /// <response code="200">Операция проведена успешно</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AccountController : Controller
    {
        private readonly AuthorizationDbContext _db;
        private readonly IAccounts _accounts;

        public AccountController(IAccounts accounts, AuthorizationDbContext db)
        {
            _db = db;
            _accounts = accounts;
        }

        /// <summary>
        /// Получить все аккаунты
        /// </summary>
        /// <response code="404">Не найдено ни одного зарегистрированного аккаунта</response>
        /// <response code="401">Доступ только для администратора</response>
        /// <returns></returns>
        [HttpGet("all")]
        //[AuthorizeEnum(Roles.administratior, Roles.superadministrator)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AccountDto>>> GetAllAccounts()
        {
            var accounts = await _accounts.GetAllAccounts();

            return Ok(accounts);
        }

        /// <summary>
        /// Получить все удаленные аккаунты
        /// </summary>
        /// <response code="404">Не найдено ни одного удаленного аккаунта</response>
        /// <response code="401">Доступ только для администратора</response>
        /// <returns></returns>
        [HttpGet("allDeleted")]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<AccountDto>>> GetAllDeletedAccounts()
        {
            var deletedAccounts = await _accounts.GetAllDeletedAccounts();

            return Ok(deletedAccounts);
        }


        /// <summary>
        /// Получить аккаунт по id
        /// </summary>
        /// <param name = "id">Идентификатор</param>
        /// <response code="404">Аккаунт не найден</response> 
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> GetCurrentAccount([Required] Guid id)
        {
            var account = await _accounts.GetAccount(id);

            if (account == null) return NotFound();

            return new AccountDto(account);
        }


        /// <summary>
        /// Удалить аккаунт
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <response code="404">Аккаунт не найден</response> 
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteAccount([Required] Guid id)
        {
            var isDeleted = await _accounts.DeleteAccount(id);
            return isDeleted ? Ok() : NotFound();
        }

        /// <summary>
        /// Создать аккаунт для слушателя
        /// </summary>
        /// <param name="listenerCreateDto"> Данные слушателя </param>
        /// <response code="409">Аккаунт с таким именем уже существует</response>
        /// <returns></returns>
        [HttpPost("listener")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AccountDto>> RegisterListenerAccount([FromBody] AccountCreateDto listenerCreateDto)
        {
            var isEqual = await _accounts.CheckNameEquality(listenerCreateDto.NickName);

            if (isEqual) return Conflict("Such name exists");

            //Мы не може переносить пароль в сыром виде на это нет соответствующего поля в моделях 
            //В данном случае можно использовать автомаппер перенося пароль отдельно, но тогда это еще хуже и грязней 
            var createdListener = await _accounts.CreateAccount(listenerCreateDto, Roles.listener);

            return Ok(createdListener);
        }

        /// <summary>
        /// Создать аккаунт для исполнителя
        /// </summary>
        /// <param name="performerCreateDto"> Данные исполнителя </param>
        /// <response code="409">Аккаунт с таким именем уже существует</response>
        /// <returns></returns>
        [HttpPost("performer")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AccountDto>> RegisterPerformerAccount([FromBody] AccountCreateDto performerCreateDto)
        {
            var isEqual = await _accounts.CheckNameEquality(performerCreateDto.NickName);

            if (isEqual) return Conflict("Such name exists");

            var createdPerformer = await _accounts.CreateAccount(performerCreateDto, Roles.performer);

            return Ok(createdPerformer);
        }


        /// <summary>
        /// Создать аккаунт для админа
        /// </summary>
        /// <param name="adminCreateDto">Данные исполнителя</param>
        /// <response code="409">Аккаунт с таким именем уже существует</response>
        /// <returns></returns>
        [HttpPost("admin")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AccountDto>> RegisterAdminAccount([FromBody] AccountCreateDto adminCreateDto)
        {
            var isEqual = await _accounts.CheckNameEquality(adminCreateDto.NickName);

            if (isEqual) return Conflict("Such name exists");

            var createdPerformer = await _accounts.CreateAccount(adminCreateDto, Roles.administratior);

            return Ok(createdPerformer);
        }


        /// <summary>
        /// Обновить аккаунт
        /// </summary>
        /// <param name="id"> Идентификатор</param>
        /// <param name="accounCreateDto"> Данные для обновления </param>
        /// <response code="404">Аккаунт не найден</response> 
        /// <response code="409">Аккаунт с таким именем уже существует</response>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult> UpdateAccount([Required] Guid id, [FromBody] AccountCreateDto accounCreateDto)
        {
            var isEqual = await _accounts.CheckNameEquality(accounCreateDto.NickName);

            if (isEqual) return Conflict("Such name exists");

            var isUpdated = await _accounts.UpdateAccount(id, accounCreateDto);

            return isUpdated ? Ok() : NotFound();
        }

        /// <summary>
        /// Восстановить аккаунт
        /// </summary>
        /// <param name="deletedAccountId "> Идентификатор </param>
        /// <response code="404">Аккаунт не найден</response> 
        /// <response code="401">Доступ только для администратора</response>
        /// <returns></returns>
        [HttpPost("{deletedAccountId}")]
        [AuthorizeEnum(Roles.administratior, Roles.superadministrator)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> RestoreAccount([Required] Guid deletedAccountId)
        {
            var isRestored = await _accounts.RestoreAccount(deletedAccountId);

            return isRestored ? Ok() : NotFound();
        }

    }
}