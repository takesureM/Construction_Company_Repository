using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationService.Models
{

    /// <summary>
    /// Роли пользователей
    /// </summary>
    public enum Roles
    {
        /// <summary>
        /// Не авторизован
        /// </summary>
        NoAuthorized,

        /// <summary>
        /// Слушатель
        /// </summary>
        listener = 100,

        /// <summary>
        /// Исполнитель
        /// </summary>
        performer = 110,

        /// <summary>
        /// Администратор 
        /// </summary>
        administratior = 120,

        /// <summary>
        /// Суперадминистратор
        /// </summary>
        superadministrator = 130,

    }
}
