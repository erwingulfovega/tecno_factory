using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using WebApi.Db;
using System.Security.Cryptography;
using WebApi.Controllers;

namespace WebApi.Models
{
    public class LoginDB
    {
        private readonly AppDb db;

        public LoginDB(AppDb db)
        {
            this.db = db;
        }

        public bool Login(Login login)
        {
            var usuarios = new UsuariosDB(db);
            try
            {
                if (login.Nombre_Usuario!=string.Empty && login.Clave!=string.Empty)
                {

                    string storedHashedPassword = usuarios.GetPasswordUserName(login.Nombre_Usuario); // Retrieve stored hash
                    string providedPassword = login.Clave; // Get user input password
                    string newHash = HashPasswordWithSHA256(providedPassword); // Hash provided password

                    bool passwordsMatch = storedHashedPassword == newHash; // Compare hashes

                    if (passwordsMatch)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error de parámetros " + ex.Source);
            }
            return true;
        }
                
        private void Parameters(MySqlCommand cmd, Login login)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@usuario",
                DbType = DbType.String,
                Value = login.Nombre_Usuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clave",
                DbType = DbType.String,
                Value = login.Clave,
            });

        }

        public string HashPasswordWithSHA256(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = sha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
   
}