using MFoot.Maui.Common;
using MFoot.Maui.Configuration;
using MFoot.Maui.Dal;
using MFoot.Maui.Domain;
using MFoot.Maui.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Aplicacao
{
    public class TimeAplicacao
    {
        private ConexaoSqLite _conexaoSqLite;

        public TimeAplicacao(ConexaoSqLite conexaoSqLite)
        {
            _conexaoSqLite = conexaoSqLite;
        }


        // ----- Time ----- //
        public void InserirTime(Time time)
        {
            var sql = new StringBuilder();
            sql.Append(" INSERT INTO times ( Id, Nome, Estadio, Capacidade, Divisao ) ");
            sql.Append(" VALUES ( ");
            sql.AppendFormat(" '{0}', ", time.Id);
            sql.AppendFormat(" '{0}', ", time.Nome);
            sql.AppendFormat(" '{0}', ", time.Estadio);
            sql.AppendFormat(" '{0}', ", time.Capacidade);
            sql.AppendFormat(" '{0}' ", time.Divisao);
            sql.Append(" ); ");

            _conexaoSqLite.ExecuteNonQuery(sql.ToString());
        }

        public Time RetornarTime(int timeId)
        {
            var sql = new StringBuilder();
            sql.Append(" SELECT * FROM times ");
            sql.AppendFormat(" where Id = '{0}' ", timeId);

            using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
            {
                if (reader.Read())
                {
                    return PopularTime(reader);
                }
                else
                {
                    return new Time();
                }
            }
        }

        public List<Time> ListarTimes()
        {
            var sql = "SELECT * FROM times";

            using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
            {
                var times = new List<Time>();

                while (reader.Read())
                {
                    times.Add(PopularTime(reader));
                }

                return times;
            }
        }

        public List<Time> ListarTimesPorDivisao(int divisao)
        {
            var sql = new StringBuilder();
            sql.Append(" SELECT * FROM times ");
            sql.AppendFormat(" where Divisao = '{0}' ", divisao);

            using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
            {
                var times = new List<Time>();

                while (reader.Read())
                {
                    times.Add(PopularTime(reader));
                }

                return times;
            }
        }

        private Time PopularTime(SqliteDataReader reader)
        {
            try
            {
                var time = new Time();
                time.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                time.Nome = reader[reader.GetOrdinal("Nome")].ToString();
                time.Divisao = reader.GetInt32(reader.GetOrdinal("Divisao"));

                time.Jogadores = ListarJogadoresPorTime(time.Id);
                time.Ataque = time.Jogadores.Sum(x => x.Ataque) / time.Jogadores.Count();
                time.Defesa = time.Jogadores.Sum(x => x.Defesa) / time.Jogadores.Count();

                time.JogadoresTitulares = time.Jogadores.Where(x => x.Titular).ToList();
                time.JogadoresReservas = time.Jogadores.Where(x => !x.Titular).ToList();
                time.AtaqueTitular = time.Jogadores.Where(x => x.Titular).Sum(x => x.Ataque) / 11;
                time.DefesaTitular = time.Jogadores.Where(x => x.Titular).Sum(x => x.Defesa) / 11;

                return time;
            }
            catch
            {
                throw;
            }
        }




        // ----- Jogador ----- //
        public void InserirJogador(Jogador jogador)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO jogadores ( Nome,Ataque, Defesa, Finalizacao, Resistencia, Posicao, Zona, DataNascimento, TimeId, Valor, Titular ) ");
                sql.Append(" VALUES ( ");
                sql.AppendFormat(" '{0}', ", jogador.Nome);
                sql.AppendFormat(" '{0}', ", jogador.Ataque);
                sql.AppendFormat(" '{0}', ", jogador.Defesa);
                sql.AppendFormat(" '{0}', ", jogador.Finalizacao);
                sql.AppendFormat(" '{0}', ", jogador.Resistencia);
                sql.AppendFormat(" '{0}', ", jogador.Posicao);
                sql.AppendFormat(" '{0}', ", jogador.Zona);
                sql.AppendFormat(" '{0}', ", Util.DateToSqLite(jogador.DataNascimento));
                sql.AppendFormat(" '{0}', ", jogador.TimeId);
                sql.AppendFormat(" '{0}', ", jogador.Valor);
                sql.AppendFormat(" '{0}' ", jogador.Titular ? 1 : 0);
                sql.Append(" ); ");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public List<Jogador> ListarJogadores()
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" SELECT *, CAST((JULIANDAY('{0}') - JULIANDAY(DataNascimento)) / 365.25 AS INTEGER) AS Idade ", GameConfiguration.DataAtual);
                sql.Append(" FROM jogadores ");

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var jogadores = new List<Jogador>();

                    while (reader.Read())
                    {
                        jogadores.Add(PopularJogador(reader));
                    }

                    return jogadores;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Jogador> ListarJogadoresPorTime(int timeId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" SELECT *, CAST((JULIANDAY('{0}') - JULIANDAY(DataNascimento)) / 365.25 AS INTEGER) AS Idade ", GameConfiguration.DataAtual);
                sql.Append(" FROM jogadores ");
                sql.AppendFormat(" WHERE TimeId = '{0}'", timeId);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var jogadores = new List<Jogador>();

                    while (reader.Read())
                    {
                        jogadores.Add(PopularJogador(reader));
                    }

                    return jogadores;
                }
            }
            catch
            {
                throw;
            }
        }                

        public List<Jogador> ListarJogadoresResistenciaBaixa()
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" SELECT *, CAST((JULIANDAY('{0}') - JULIANDAY(DataNascimento)) / 365.25 AS INTEGER) AS Idade ", GameConfiguration.DataAtual);
                sql.Append(" FROM jogadores ");
                sql.AppendFormat(" WHERE Resistencia < 70.00 ");
                sql.AppendFormat(" AND Titular = 1 ");

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var jogadores = new List<Jogador>();

                    while (reader.Read())
                    {
                        jogadores.Add(PopularJogador(reader));
                    }

                    return jogadores;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Jogador> ListarJogadoresSuspensos(bool titulares)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" SELECT t1.*, CAST((JULIANDAY('{0}') - JULIANDAY(DataNascimento)) / 365.25 AS INTEGER) AS Idade ", GameConfiguration.DataAtual);
                sql.Append(" FROM jogadores t1 ");
                sql.AppendFormat(" LEFT JOIN jogadores_suspensoes t2 on t2.JogadorId = t1.Id ");
                sql.AppendFormat(" WHERE t1.Titular = {0} and t2.Concluido = 0 ", titulares ? 1 : 0);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var jogadores = new List<Jogador>();

                    while (reader.Read())
                    {
                        jogadores.Add(PopularJogador(reader));
                    }

                    return jogadores;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<Jogador> ListarJogadoresComCartao(int tipoCartao, int qtdCartoes, int campeonatoId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" SELECT t1.*, CAST((JULIANDAY('{0}') - JULIANDAY(DataNascimento)) / 365.25 AS INTEGER) AS Idade ", GameConfiguration.DataAtual);
                sql.Append(" FROM jogadores t1 ");
                sql.AppendFormat(" WHERE (SELECT COUNT(*) FROM jogadores_cartoes WHERE JogadorId = t1.Id and TipoId = {0} and Concluido = 0 and CampeonatoId = {0}  ) = {0} ", tipoCartao, qtdCartoes, campeonatoId);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var jogadores = new List<Jogador>();

                    while (reader.Read())
                    {
                        jogadores.Add(PopularJogador(reader));
                    }

                    return jogadores;
                }
            }
            catch
            {
                throw;
            }
        }

        public Jogador RetornarJogador(int jogadorId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" SELECT *, CAST((JULIANDAY('{0}') - JULIANDAY(DataNascimento)) / 365.25 AS INTEGER) AS Idade ", GameConfiguration.DataAtual);
                sql.Append(" FROM jogadores ");
                sql.AppendFormat(" WHERE Id = '{0}' ", jogadorId);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return PopularJogador(reader);
                    }
                    else
                    {
                        return new Jogador();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public Jogador RetornarJogadorSubstitituto(int timeId, string posicao, string zona)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" SELECT *, CAST((JULIANDAY('{0}') - JULIANDAY(DataNascimento)) / 365.25 AS INTEGER) AS Idade ", GameConfiguration.DataAtual);
                sql.Append(" FROM jogadores ");
                sql.AppendFormat(" WHERE TimeId = '{0}' ", timeId);
                sql.AppendFormat(" and (Posicao = '{0}' or Zona = '{1}') ", posicao, zona);
                sql.Append(" and Titular = 0 ");
                sql.AppendFormat(" order by Posicao = '{0}' desc, Resistencia desc, (Ataque + Defesa + Finalizacao) desc ", posicao);
                sql.Append(" limit 1 ");

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return PopularJogador(reader);
                    }
                    else
                    {
                        return new Jogador();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void AtualizaJogadoresVitoria(int timeId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE jogadores ");
                sql.Append(" SET ");
                sql.Append(" Ataque = ROUND(Ataque + 0.10, 2), ");
                sql.Append(" Defesa = ROUND(Defesa + 0.10, 2) ");
                sql.AppendFormat(" WHERE TimeId = '{0}' ", timeId);

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void AtualizaJogadoresDerrota(int timeId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE jogadores ");
                sql.Append(" SET ");
                sql.Append(" Ataque = ROUND(Ataque - 0.05, 2), ");
                sql.Append(" Defesa = ROUND(Defesa - 0.05, 2) ");
                sql.AppendFormat(" WHERE TimeId = '{0}' ", timeId);

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void AtualizaTitularidadeJogador(Jogador jogador, bool titular)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE jogadores ");
                sql.Append(" SET ");
                sql.AppendFormat(" Titular = '{0}' ", titular ? 1 : 0);
                sql.AppendFormat(" WHERE Id = '{0}' ", jogador.Id);

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void AtualizaDadosJogador(Jogador jogador)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE jogadores ");
                sql.Append(" SET ");
                sql.AppendFormat(" Resistencia = '{0}' ", jogador.Resistencia);
                sql.AppendFormat(" WHERE Id = '{0}' ", jogador.Id);

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void SubstituirJogadoresResistenciaBaixa()
        {
            try
            {
                var jogadoresResitenciaBaixa = ListarJogadoresResistenciaBaixa();

                foreach (var jogador in jogadoresResitenciaBaixa)
                {
                    var jogadorQueSai = jogador;
                    var jogadorQueEntra = RetornarJogadorSubstitituto(jogador.TimeId, jogador.Posicao, jogador.Zona);

                    AtualizaTitularidadeJogador(jogadorQueSai, false);
                    AtualizaTitularidadeJogador(jogadorQueEntra, true);
                }
            }
            catch
            {
                throw;
            }
        }

        public void SubstituirJogadoresSuspensos()
        {
            try
            {
                var jogadoresSuspensos = ListarJogadoresSuspensos(true);

                foreach (var jogador in jogadoresSuspensos)
                {
                    var jogadorQueSai = jogador;
                    var jogadorQueEntra = RetornarJogadorSubstitituto(jogador.TimeId, jogador.Posicao, jogador.Zona);

                    AtualizaTitularidadeJogador(jogadorQueSai, false);
                    AtualizaTitularidadeJogador(jogadorQueEntra, true);
                }
            }
            catch
            {
                throw;
            }
        }

        public void RecuperarResistenciaJogadoresPosRodada()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE jogadores ");
                sql.Append(" SET ");
                sql.AppendFormat(" Resistencia = MIN(100.00, ROUND(Resistencia + ((CAST((JULIANDAY('{0}') - JULIANDAY(DataNascimento)) / 365.25 AS REAL) / 60) * 75),2)) ", GameConfiguration.DataAtual);

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        private Jogador PopularJogador(SqliteDataReader reader)
        {
            try
            {
                var jogador = new Jogador();
                jogador.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                jogador.Nome = reader[reader.GetOrdinal("Nome")].ToString();
                jogador.Ataque = reader[reader.GetOrdinal("Ataque")] != DBNull.Value ? double.Parse(reader[reader.GetOrdinal("Ataque")].ToString()) : 0.00;
                jogador.Defesa = reader[reader.GetOrdinal("Defesa")] != DBNull.Value ? double.Parse(reader[reader.GetOrdinal("Defesa")].ToString()) : 0.00;
                jogador.Finalizacao = reader[reader.GetOrdinal("Finalizacao")] != DBNull.Value ? double.Parse(reader[reader.GetOrdinal("Finalizacao")].ToString()) : 0.00;
                jogador.Resistencia = reader[reader.GetOrdinal("Resistencia")] != DBNull.Value ? double.Parse(reader[reader.GetOrdinal("Resistencia")].ToString()) : 0.00;
                jogador.Posicao = reader[reader.GetOrdinal("Posicao")].ToString();
                jogador.Zona = reader[reader.GetOrdinal("Zona")].ToString();
                jogador.Valor = reader[reader.GetOrdinal("Valor")] != DBNull.Value ? double.Parse(reader[reader.GetOrdinal("Valor")].ToString()) : 0.00;
                jogador.TimeId = reader[reader.GetOrdinal("TimeId")] != DBNull.Value ? reader.GetInt32("TimeId") : 0;
                jogador.Titular = reader.GetInt32(reader.GetOrdinal("Titular")) == 1 ? true : false;
                jogador.Idade = reader[reader.GetOrdinal("Idade")] != DBNull.Value ? reader.GetInt32("Idade") : 0;

                jogador.Cartoes = ListarCartoesPorJogador(jogador.Id);

                return jogador;
            }
            catch
            {
                throw;
            }
        }



        // ----- Cartões ----- //
        public void InserirJogadorCartao(JogadorCartao cartao)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO jogadores_cartoes ( TipoId, CampeonatoId, TimeId, JogadorId, Concluido ) ");
                sql.Append(" VALUES ( ");
                sql.AppendFormat(" '{0}', ", cartao.TipoId);
                sql.AppendFormat(" '{0}', ", cartao.CampeonatoId);
                sql.AppendFormat(" '{0}', ", cartao.TimeId);
                sql.AppendFormat(" '{0}', ", cartao.JogadorId);
                sql.AppendFormat(" '{0}' ", cartao.Concluido ? 1 : 0);
                sql.Append(" ); ");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public List<JogadorCartao> ListarCartoesPorJogador(int jogadorId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.AppendFormat(" SELECT * ");
                sql.Append(" FROM jogadores_cartoes ");
                sql.AppendFormat(" WHERE JogadorId = '{0}'", jogadorId);
                sql.Append(" AND Concluido = '0' ");

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var cartoes = new List<JogadorCartao>();

                    while (reader.Read())
                    {
                        cartoes.Add(PopularJogadorCartao(reader));
                    }

                    return cartoes;
                }
            }
            catch
            {
                throw;
            }
        }

        public void RemoveJogadorCartaoAmarelo(JogadorCartao cartao)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE jogadores_cartoes ");
                sql.Append(" SET ");
                sql.Append(" Concluido = '1' ");
                sql.Append(" WHERE TipoId = '2' ");
                sql.AppendFormat(" AND CampeonatoId = '{0}' ", cartao.CampeonatoId);
                sql.AppendFormat(" AND JogadorId = '{0}' ", cartao.JogadorId);
                sql.Append(" AND Concluido = '0' ");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        private JogadorCartao PopularJogadorCartao(SqliteDataReader reader)
        {
            try
            {
                var jogador = new JogadorCartao();
                jogador.Id = reader.GetInt32(reader.GetOrdinal("Id"));                
                jogador.TipoId = reader.GetInt32(reader.GetOrdinal("TipoId"));                
                jogador.CampeonatoId = reader.GetInt32(reader.GetOrdinal("CampeonatoId"));                
                jogador.TimeId = reader.GetInt32(reader.GetOrdinal("TimeId"));                
                jogador.JogadorId = reader.GetInt32(reader.GetOrdinal("JogadorId"));                
                jogador.Concluido = reader.GetInt32(reader.GetOrdinal("Concluido")) == 1 ? true : false;

                return jogador;
            }
            catch
            {
                throw;
            }
        }


        // ----- Cartões ----- //
        public void InserirJogadorSuspensao(JogadorSuspensao suspensao)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO jogadores_suspensoes ( CampeonatoId, TimeId, JogadorId, QuantidadeJogos, Concluido ) ");
                sql.Append(" VALUES ( ");
                sql.AppendFormat(" '{0}', ", suspensao.CampeonatoId);
                sql.AppendFormat(" '{0}', ", suspensao.TimeId);
                sql.AppendFormat(" '{0}', ", suspensao.JogadorId);
                sql.AppendFormat(" '{0}', ", suspensao.QuantidadeJogos);
                sql.AppendFormat(" '{0}' ", suspensao.Concluido ? 1 : 0);
                sql.Append(" ); ");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void RemoverJogadorSuspensao(JogadorSuspensao suspensao)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE jogadores_suspensoes ");
                sql.Append(" SET Concluido = '1' ");
                sql.AppendFormat(" WHERE JogadorId = '{0}' ", suspensao.JogadorId);
                sql.AppendFormat(" AND CampeonatoId = '{0}' ", suspensao.CampeonatoId);
                sql.Append(" AND QuantidadeJogos = '1' ");
                sql.Append(" AND Concluido = '0' ");


                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }
    }
}
