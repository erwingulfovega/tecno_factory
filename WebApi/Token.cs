using System.Security.Cryptography;
using System.Text;

namespace WebApi
{
    public class Token
    {

        public string HashToken { get; set; }
        public Token()
        {
            HashToken= string.Empty;
        }
    }
}
