using MFoot.Maui.Configuration;
using MFoot.Maui.Dal;
using MFoot.Maui.Models;
using System.Text;

namespace MFoot.Maui.Aplicacao
{
    public class DatabaseAplicacao
    {
        private readonly ConexaoSqLite _conexaoSqLite;

        public DatabaseAplicacao(ConexaoSqLite conexaoSqLite)
        {
            _conexaoSqLite = conexaoSqLite;
        }

        // ----- DataBase ----- //
        public void InserirTabelaTime()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS times ( ");
                sql.Append(" Id INTEGER PRIMARY KEY, ");
                sql.Append(" Nome TEXT NOT NULL, ");
                sql.Append(" Estadio TEXT NOT NULL, ");
                sql.Append(" Capacidade TEXT NOT NULL, ");
                sql.Append(" Divisao INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaJogador()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS jogadores ( ");
                sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
                sql.Append(" Nome TEXT NOT NULL, ");
                sql.Append(" Ataque REAL, ");
                sql.Append(" Defesa REAL, ");
                sql.Append(" Finalizacao REAL, ");
                sql.Append(" Resistencia REAL, ");
                sql.Append(" Posicao TEXT, ");
                sql.Append(" Zona TEXT, ");
                sql.Append(" DataNascimento TEXT, ");
                sql.Append(" TimeId INTEGER, ");
                sql.Append(" Valor REAL, ");
                sql.Append(" Titular INTEGER ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaJogadorCartoes()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS jogadores_cartoes ( ");
                sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
                sql.Append(" TipoId INTEGER NOT NULL, ");
                sql.Append(" CampeonatoId INTEGER NOT NULL, ");
                sql.Append(" TimeId INTEGER NOT NULL, ");
                sql.Append(" JogadorId INTEGER NOT NULL, ");
                sql.Append(" Concluido INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaJogadorSuspensoes()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS jogadores_suspensoes ( ");
                sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
                sql.Append(" CampeonatoId INTEGER NOT NULL, ");
                sql.Append(" TimeId INTEGER NOT NULL, ");
                sql.Append(" JogadorId INTEGER NOT NULL, ");
                sql.Append(" QuantidadeJogos INTEGER NOT NULL, ");
                sql.Append(" Concluido INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaTemporada()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS temporadas ( ");
                sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
                sql.Append(" Ano INTEGER NOT NULL, ");
                sql.Append(" DataInicio TEXT NOT NULL, ");
                sql.Append(" Atual INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaCampeonato()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS campeonatos ( ");
                sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
                sql.Append(" Tipo TEXT NOT NULL, ");
                sql.Append(" TemporadaId INTEGER NOT NULL, ");
                sql.Append(" DataInicio TEXT NOT NULL, ");
                sql.Append(" TimesParticipantes INTEGER NOT NULL, ");
                sql.Append(" TimesPromocao INTEGER NOT NULL, ");
                sql.Append(" TimesRebaixamento INTEGER NOT NULL, ");
                sql.Append(" Divisao INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaCampeonatoClassificacao()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS campeonatos_classificacao ( ");
                sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
                sql.Append(" CampeonatoId INTEGER NOT NULL, ");
                sql.Append(" TimeId INTEGER NOT NULL, ");
                sql.Append(" Jogos INTEGER NOT NULL, ");
                sql.Append(" Pontos INTEGER NOT NULL, ");
                sql.Append(" Vitoria INTEGER NOT NULL, ");
                sql.Append(" Empate INTEGER NOT NULL, ");
                sql.Append(" Derrota INTEGER NOT NULL, ");
                sql.Append(" GolFavor INTEGER NOT NULL, ");
                sql.Append(" GolContra INTEGER NOT NULL, ");
                sql.Append(" CartaoAmarelo INTEGER NOT NULL, ");
                sql.Append(" CartaoVermelho INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaCampeonatoRodada()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS campeonatos_rodadas ( ");
                sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
                sql.Append(" CampeonatoId INTEGER NOT NULL, ");
                sql.Append(" Numero INTEGER NOT NULL, ");
                sql.Append(" Turno INTEGER NOT NULL, ");
                sql.Append(" Data TEXT NOT NULL, ");
                sql.Append(" Concluida INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaCampeonatoPartida()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS campeonatos_partidas ( ");
                sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
                sql.Append(" RodadaId INTEGER NOT NULL, ");
                sql.Append(" TimeCasaId INTEGER NOT NULL, ");
                sql.Append(" TimeVisitanteId INTEGER NOT NULL, ");
                sql.Append(" PlacarCasa INTEGER NOT NULL, ");
                sql.Append(" PlacarVisitante INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaCampeonatoPartidaEvento()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS campeonatos_partidas_eventos ( ");
                sql.Append(" Id INTEGER PRIMARY KEY AUTOINCREMENT, ");
                sql.Append(" TipoId INTEGER NOT NULL, ");
                sql.Append(" PartidaId INTEGER NOT NULL, ");
                sql.Append(" TimeId INTEGER NOT NULL, ");
                sql.Append(" JogadorId INTEGER NOT NULL, ");
                sql.Append(" Tempo INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirTabelaCampeonatoPartidaEventoTipo()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" CREATE TABLE IF NOT EXISTS campeonatos_partidas_eventos_tipos ( ");
                sql.Append(" Id INTEGER PRIMARY KEY , ");
                sql.Append(" Tipo INTEGER NOT NULL ");
                sql.Append(" );");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void InserirCampeonatoPartidaEventoTipo()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO campeonatos_partidas_eventos_tipos ( Id, Tipo ) VALUES ( '1', 'Gol' ); ");
                sql.Append(" INSERT INTO campeonatos_partidas_eventos_tipos ( Id, Tipo ) VALUES ( '2', 'Cartão Amarelo' ); ");
                sql.Append(" INSERT INTO campeonatos_partidas_eventos_tipos ( Id, Tipo ) VALUES ( '3', 'Cartão Vermelho' ); ");                

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }
    }
}
