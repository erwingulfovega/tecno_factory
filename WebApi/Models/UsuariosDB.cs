using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using WebApi.Db;
using System.Security.Cryptography;

namespace WebApi.Models
{
    public class UsuariosDB
    {
        private readonly AppDb db;

        public UsuariosDB(AppDb db)
        {
            this.db = db;
        }

        public Usuarios GetId(int id)
        {
            var result = new List<Usuarios>();
            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = string.Format("SELECT id," + Environment.NewLine +
                          "documento_identidad," + Environment.NewLine +
                          "nombres," + Environment.NewLine +
                          "apellidos," + Environment.NewLine +
                          "email," +
                          "nombre_usuario," +
                          "clave FROM usuarios where id={0}", id);

                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                result = LoadList(cmd.ExecuteReader());

                if (result.Count > 1)
                {
                    throw new Exception("Hay más de un usuario con el mismo identificador");
                }
                else
                {
                    return result[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los usuarios " + ex.Source);
            }

        }

        public Usuarios GetUserByDocumentIdentification(string Identification)
        {
            
            var result = new List<Usuarios>();
            
            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = string.Format("SELECT id," + Environment.NewLine +
                          "documento_identidad," + Environment.NewLine +
                          "nombres," + Environment.NewLine +
                          "apellidos," + Environment.NewLine +
                          "email," +
                          "nombre_usuario," + Environment.NewLine +
                          "clave " + Environment.NewLine +
                          "FROM usuarios where documento_identidad={0}",Identification);
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                result = LoadList(cmd.ExecuteReader());

                if (result.Count>1)
                {
                    throw new Exception("Hay más de un usuario con el mismo documento de identidad");
                }
                else
                {
                    return result[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los usuarios "+ ex.Source);
            }
            
        }

        public Usuarios GetUserByEmail(string Email)
        {

            var result = new List<Usuarios>();

            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = string.Format("SELECT id," + Environment.NewLine +
                          "documento_identidad," + Environment.NewLine +
                          "nombres," + Environment.NewLine +
                          "apellidos," + Environment.NewLine +
                          "email," +
                          "nombre_usuario," + Environment.NewLine +
                          "clave " + Environment.NewLine +
                          "FROM usuarios where email={0}", Email);
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                result = LoadList(cmd.ExecuteReader());

                if (result.Count > 1)
                {
                    throw new Exception("Hay más de un usuario con el mismo documento de identidad");
                }
                else
                {
                    return result[0];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los usuarios " + ex.Source);
            }

        }

        public string GetPasswordUserName(string Nombre_Usuario)
        {

            var result = new List<Usuarios>();

            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = string.Format("SELECT id," + Environment.NewLine +
                          "documento_identidad," + Environment.NewLine +
                          "nombres," + Environment.NewLine +
                          "apellidos," + Environment.NewLine +
                          "email," +
                          "nombre_usuario," + Environment.NewLine +
                          "clave " + Environment.NewLine +
                          "FROM usuarios where nombre_usuario='{0}'", Nombre_Usuario);
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                result = LoadList(cmd.ExecuteReader());

                if (result.Count > 1)
                {
                    throw new Exception("Hay más de un usuario con el mismo documento de identidad");
                }
                else
                {
                    return result[0].Clave;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los usuarios " + ex.Source);
            }

        }

        public IEnumerable<Usuarios> GetUsuarios()
        {
            var result = new List<Usuarios>();
            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = "SELECT id," + Environment.NewLine +
                          "documento_identidad," + Environment.NewLine+
                          "nombres," + Environment.NewLine+
                          "apellidos," +
                          "email," + Environment.NewLine +
                          "nombre_usuario," + Environment.NewLine +
                          "clave" + Environment.NewLine+
                          "FROM usuarios";
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                result = LoadList(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los usuarios " + ex.Source);
            }
            return result;
        }

        public int Insert(Usuarios usuarios)
        {
            int Id = 0;
            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = string.Format(@"INSERT INTO usuarios " +
                                   "(documento_identidad," +
                                   " nombres," +
                                   " apellidos,"+
                                   " email," +
                                   " nombre_usuario," +
                                   " clave)" +
                                   " VALUES (@documento_identidad," +
                                   "@nombres," +
                                   "@apellidos," +
                                   "@nombre_usuario," +
                                   "@email," +
                                   "@nombre_usuario," +
                                   "@clave);");
                cmd.CommandText= sql;
                Parameters(cmd, usuarios);
                cmd.ExecuteNonQuery();
                Id = (int)cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el usuario " + ex.Source);
            }
            return Id;

        }

        public void Update(Usuarios usuarios)
        {

            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = string.Format(@" UPDATE usuarios " +
                                  " SET documento_identidad = @documento_identidad," +
                                  " nombres = @nombres," +
                                  " apellidos = @apellidos," +
                                  " email = @email," +
                                  " nombre_usuario = @nombre_usuario,"+
                                  " clave = @clave" +
                                  " WHERE " +
                                  " id = @id;");
                cmd.CommandText= sql;
                Parameters(cmd, usuarios);
                cmd.Parameters.AddWithValue("@id", usuarios.Id);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario " + ex.Source);
            }

        }

        public void Delete(int id)
        {

            try
            {
                using var cmd = db.Connection.CreateCommand();
                cmd.CommandText = "START TRANSACTION;";
                cmd.CommandText += string.Format(@"DELETE FROM usuarios WHERE id = @Id;");
                cmd.CommandText += "COMMIT;";

                cmd.Parameters.AddWithValue("@Id", id);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario " + ex.Source);
            }

        }

        private List<Usuarios> LoadList(System.Data.Common.DbDataReader reader)
        {
            List<Usuarios> lista = new List<Usuarios>();

            using (reader)
            {
                while (reader.Read())
                {
                    Usuarios clase = new Usuarios();
                    clase.Id = (!reader.IsDBNull("documento_identidad")) ? reader.GetInt32("id") : 0;
                    clase.DocumentoIdentidad = (!reader.IsDBNull("documento_identidad")) ? reader.GetString("documento_identidad") : string.Empty;
                    clase.Nombres = (!reader.IsDBNull("nombres")) ? reader.GetString("nombres") : string.Empty;
                    clase.Apellidos = (!reader.IsDBNull("apellidos")) ? reader.GetString("apellidos") : string.Empty;
                    clase.Email = (!reader.IsDBNull("email")) ? reader.GetString("email") : string.Empty;
                    clase.Nombre_Usuario = (!reader.IsDBNull("nombre_usuario")) ? reader.GetString("nombre_usuario") : string.Empty;
                    clase.Clave = (!reader.IsDBNull("clave")) ? reader.GetString("clave") : string.Empty;
                    lista.Add(clase);
                }
            }
            return lista;
        }

        private void Parameters(MySqlCommand cmd, Usuarios usuarios)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@documento_identidad",
                DbType = DbType.String,
                Value = usuarios.DocumentoIdentidad,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@nombres",
                DbType = DbType.String,
                Value = usuarios.Nombres,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@apellidos",
                DbType = DbType.String,
                Value = usuarios.Apellidos,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@email",
                DbType = DbType.String,
                Value = usuarios.Email,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@nombre_usuario",
                DbType = DbType.String,
                Value = usuarios.Nombre_Usuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@clave",
                DbType = DbType.String,
                Value = HashPasswordWithSHA256(usuarios.Clave),
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