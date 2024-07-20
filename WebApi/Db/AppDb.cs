using System.Data.Common;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace WebApi.Db
{
    public class AppDb : IDisposable
    {
        public MySqlConnection Connection { get; }
        private string _connectionString;
        public string DbName { get; set; }

        public AppDb(string connectionString)
        {

            try
            {
                this._connectionString = connectionString;
                Connection = new MySqlConnection(_connectionString);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al instanciar BBDD" + ex.Message.ToString());
            }
        }

        public bool checkConnection()
        {

            try
            {
                Connection.Open();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al checkConnection" + ex.Message.ToString());
                return false;
            }
        }
        public void Dispose() => Connection.Dispose();
    }

}