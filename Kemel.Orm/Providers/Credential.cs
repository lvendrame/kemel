using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kemel.Orm.Providers
{
    public enum AuthenticationMode
    {
        WindowsADUser,
        SqlUser
    }

    public abstract class Credential
    {
        #region Properties

        /// <summary>
        /// Nome da Aplicação
        /// </summary>
        public abstract string ApplicationName { get; set; }

        /// <summary>
        /// Nome ou IP do Servidor
        /// </summary>
        public abstract string DataSource { get; set; }

        /// <summary>
        /// Usuário do servidor
        /// </summary>
        public abstract string User { get; set; }

        /// <summary>
        /// Senha de acesso ao servidor
        /// </summary>
        public abstract string Password { get; set; }

        /// <summary>
        /// DataBase
        /// </summary>
        public abstract string Catalog { get; set; }

        /// <summary>
        /// Forma de autenticação - Windows ou SQL
        /// </summary>
        public abstract AuthenticationMode AuthenticationMode { get; set; }

        /// <summary>
        /// DB Owner
        /// </summary>
        public abstract string Owner { get; set; }

        /// <summary>
        /// Número da porta
        /// </summary>
        public abstract int Port { get; set; }

        /// <summary>
        /// Connection TimeOut
        /// </summary>
        public abstract int ConnectionTimeOut { get; set; }

        /// <summary>
        /// Key Provider
        /// </summary>
        public abstract string KeyProvider { get; set; }

        #endregion
    }
}
