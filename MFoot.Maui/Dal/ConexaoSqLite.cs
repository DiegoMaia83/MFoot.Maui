using MFoot.Maui.Configuration;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.SqlTypes;

namespace MFoot.Maui.Dal
{
    public class ConexaoSqLite : IDisposable
    {
        private SqliteConnection _connection;
        private string _connectionString;

        public ConexaoSqLite()
        {
            // Caminho completo do arquivo de banco de dados
            

            // Inicializar o provedor SQLite
            SQLitePCL.Batteries.Init();

            /*
            // Inicializar a conexão
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
            */
        }

        public SqliteConnection GetConexao()
        {
            _connectionString = $"Data Source={Path.Combine(DbConfiguration.PathtDb, Preferences.Get("currentGameDatabase", "default.db"))};";

            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        public int ExecuteNonQuery(string sqlString)
        {
            using (var connection = GetConexao())
            {
                connection.Open();

                // Executa o comando SQL de inserção/atualização/exclusão
                using (var command = new SqliteCommand(sqlString, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Obtém o ID da última linha inserida
                using (var command = new SqliteCommand("SELECT last_insert_rowid()", connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public SqliteDataReader ExecuteReader(string selectQuery)
        {
            // Cria uma nova conexão
            var connection = GetConexao();
            connection.Open();

            // Cria o comando usando a nova conexão, sem usar o bloco using
            var command = new SqliteCommand(selectQuery, connection);

            // Executa o comando e retorna o SqliteDataReader
            // O SqliteDataReader precisa ser usado fora do método e a conexão será fechada automaticamente quando o reader for fechado
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
