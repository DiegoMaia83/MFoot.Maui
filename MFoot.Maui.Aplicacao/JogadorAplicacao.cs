using MFoot.Maui.Dal;
using MFoot.Maui.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Aplicacao
{
    public class JogadorAplicacao
    {
        private ConexaoSqLite _conexaoSqLite;

        public JogadorAplicacao()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string dbDirectory = Path.Combine(appDataPath, "MFoot");

            string dbPath = Path.Combine(dbDirectory, "m_foot.db");

            _conexaoSqLite = new ConexaoSqLite(dbPath);
        }

        public void InserirTabelaJogador()
        {
            var sql = new StringBuilder();
            sql.Append(" CREATE TABLE IF NOT EXISTS jogadores ( ");
            sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
            sql.Append(" Nome TEXT NOT NULL, ");
            sql.Append(" Ataque REAL, ");
            sql.Append(" Defesa REAL, ");
            sql.Append(" Finalizacao REAL, ");
            sql.Append(" Zona TEXT, ");
            sql.Append(" TimeId INTEGER ");
            sql.Append(" );");

            _conexaoSqLite.ExecuteNonQuery(sql.ToString());
        }        
        
        public void InserirJogador(Jogador jogador)
        {
            var sql = new StringBuilder();
            sql.Append(" INSERT INTO jogadores ( Nome,Ataque, Defesa, Finalizacao, Zona, TimeId ) ");
            sql.Append(" VALUES ( ");
            sql.AppendFormat(" '{0}', ", jogador.Nome);
            sql.AppendFormat(" '{0}', ", jogador.Ataque);
            sql.AppendFormat(" '{0}', ", jogador.Defesa);
            sql.AppendFormat(" '{0}', ", jogador.Finalizacao);
            sql.AppendFormat(" '{0}', ", jogador.Zona);
            sql.AppendFormat(" '{0}' ", jogador.TimeId);
            sql.Append(" ); ");

            _conexaoSqLite.ExecuteNonQuery(sql.ToString());
        }

        public IEnumerable<Jogador> ListarJogadores()
        {
            var sql = "SELECT * FROM jogadores";

            var reader = _conexaoSqLite.ExecuteReader(sql);

            var jogadores = new List<Jogador>();

            while (reader.Read())
            {
                var jogador = new Jogador();
                jogador.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                jogador.Nome = reader[reader.GetOrdinal("Nome")].ToString();
                jogador.Ataque = reader[reader.GetOrdinal("Ataque")] != DBNull.Value ? reader.GetDouble("Ataque") : 0.00;
                jogador.Defesa = reader[reader.GetOrdinal("Defesa")] != DBNull.Value ? reader.GetDouble("Defesa") : 0.00;
                jogador.Finalizacao = reader[reader.GetOrdinal("Finalizacao")] != DBNull.Value ? reader.GetDouble("Finalizacao") : 0.00;
                jogador.Zona = reader[reader.GetOrdinal("Zona")].ToString();
                jogador.TimeId = reader[reader.GetOrdinal("TimeId")] != DBNull.Value ? reader.GetInt32("TimeId") : 0;

                jogadores.Add(jogador);
            }

            return jogadores;
        }
    }
}
