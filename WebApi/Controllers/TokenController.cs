using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {

        [HttpGet(Name = "Token")]
        public IActionResult Get(string public_key, string private_key, string ts)
        {
            try
            {
                if (public_key != string.Empty && private_key != string.Empty && ts != string.Empty)
                {

                    var cadena = ts + private_key + public_key;

                    string algorithmName = "MD5"; // Replace with your chosen algorithm
                    HashAlgorithm hashAlgorithm = HashAlgorithm.Create(algorithmName);

                    byte[] inputBytes = Encoding.UTF8.GetBytes(cadena);
                    byte[] hashBytes = hashAlgorithm.ComputeHash(inputBytes);

                    string hashString = "";

                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        hashString += hashBytes[i].ToString("x2");
                    }

                    var hash = String.Empty;
                    Token token = new Token();
                    token.HashToken = hashString;

                    return new OkObjectResult(token);
                }
                else
                {
                    return new BadRequestObjectResult("Error de parámetros");
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Se produjo una excepción: " + ex.Message);
            }

        }
    }
}
