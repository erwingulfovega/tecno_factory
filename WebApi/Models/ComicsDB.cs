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
    public class ComicsDB
    {
        private readonly AppDb db;

        public ComicsDB(AppDb db)
        {
            this.db = db;
        }

        public Comics GetId(int id)
        {
            var result = new List<Comics>();
            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = string.Format("SELECT id," + Environment.NewLine +
                          "id_usuario," + Environment.NewLine +
                          "id_comics" + Environment.NewLine +
                          "FROM comic_favoritos where id={0}", id);

                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                result = LoadList(cmd.ExecuteReader());

                if (result.Count > 1)
                {
                    throw new Exception("Hay más de un comics con el mismo identificador");
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

        public IEnumerable<Comics> GetComicsFavoritesByUser(int idusuario)
        {
            var result = new List<Comics>();
            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = string.Format("SELECT id," + Environment.NewLine +
                          "id_usuario," + Environment.NewLine+
                          "id_comics" + Environment.NewLine+
                          "FROM comic_favoritos where id_usuario={0}", idusuario);
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                result = LoadList(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los comics " + ex.Source);
            }
            return result;
        }

        public int Insert(Comics comics)
        {
            int Id = 0;
            try
            {
                using var cmd = db.Connection.CreateCommand();
                var sql = string.Format(@"INSERT INTO comic_favoritos " +
                                   "(id_usuario," +
                                   " id_comics)" +
                                   " VALUES (@id_usuario," +
                                   "@id_comics);");
                cmd.CommandText= sql;
                Parameters(cmd, comics);
                cmd.ExecuteNonQuery();
                Id = (int)cmd.LastInsertedId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el comics " + ex.Source);
            }
            return Id;

        }
                
        public void Delete(int id)
        {

            try
            {
                using var cmd = db.Connection.CreateCommand();
                cmd.CommandText = "START TRANSACTION;";
                cmd.CommandText += string.Format(@"DELETE FROM comic_favoritos WHERE id = @Id;");
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

        private List<Comics> LoadList(System.Data.Common.DbDataReader reader)
        {
            List<Comics> lista = new List<Comics>();

            using (reader)
            {
                while (reader.Read())
                {
                    Comics clase = new Comics();
                    clase.Id = (!reader.IsDBNull("id")) ? reader.GetInt32("id") : 0;
                    clase.IdComics = (!reader.IsDBNull("id_comics")) ? reader.GetInt32("id_comics") : 0;
                    clase.IdUsuario = (!reader.IsDBNull("id_usuario")) ? reader.GetInt32("id_usuario") : 0;
                    lista.Add(clase);
                }
            }
            return lista;
        }

        private void Parameters(MySqlCommand cmd, Comics comics)
        {

            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = comics.Id,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_usuario",
                DbType = DbType.Int32,
                Value = comics.IdUsuario,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id_comics",
                DbType = DbType.Int32,
                Value = comics.IdComics,
            });
            
   
        }
       
    }
   
}