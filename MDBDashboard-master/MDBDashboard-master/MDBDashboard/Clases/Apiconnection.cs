using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MDBinASP
{
    public class ApiConection
    {
        public static HttpRequestMessage GetHttpRequestMessage(HttpMethod tipo, string action)
        {
            var urlAPI = string.Empty;
            urlAPI = System.Configuration.ConfigurationManager.AppSettings["endpoint"];
                        
            try
            {
                var separador = urlAPI.EndsWith("/") ? "" : "/";
                var requestMessage =
                    new HttpRequestMessage(tipo, string.Format("{0}{1}{2}", urlAPI, separador, action));
                return requestMessage;
               
            }
            catch (Exception ex)
            {
                throw new Exception("Error GetHttpRequestMessage: " + ex.Message);
            }
        }
    }
}
