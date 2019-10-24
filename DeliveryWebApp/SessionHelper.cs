using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp
{
    /// <summary>
    /// Esta classe auxilia a manter os itens do carrinho em
    /// uma sessão de usuário. Isso permite descartar automaticamente
    /// os itens do carrinho caso o usuário saia do site antes de
    /// concluir seu pedido para que ele seja gravado no banco de dados
    /// </summary>
    public static class SessionHelper
    {

        /// <summary>
        /// Este método converte o objeto de dados para Json e o salva
        /// temporariamente
        /// </summary>
        /// <param name="session">Objeto que representa a sessão do usuário atual</param>
        /// <param name="key">Identificador para indexar o objeto que queremos armazenar</param>
        /// <param name="value">Objeto de dados que queremos armazenar</param>
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }


        /// <summary>
        /// Este método permite recuperar os itens armazenados
        /// para a sessão de usuário informada
        /// </summary>
        /// <typeparam name="T">Tipo de dados do objeto armazenado</typeparam>
        /// <param name="session">Objeto que representa a sessão do usuário atual</param>
        /// <param name="key">Identificador do objeto armazenado</param>
        /// <returns></returns>
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
