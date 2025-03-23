using MFoot.Maui.Configuration;
using MFoot.Maui.Dal;
using MFoot.Maui.Domain;
using MFoot.Maui.Models;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;

namespace MFoot.Maui.Aplicacao
{
    public class CampeonatoAplicacao
    {
        private ConexaoSqLite _conexaoSqLite;
        private TimeAplicacao _timeAplicacao;

        public CampeonatoAplicacao(ConexaoSqLite conexaoSqLite, TimeAplicacao timeAplicacao)
        {
            _conexaoSqLite = conexaoSqLite;
            _timeAplicacao = timeAplicacao;
        }


        // ----- Campeonato ----- //
        public int InserirCampeonato(Campeonato campeonato)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO campeonatos ( Tipo, TemporadaId, DataInicio, TimesParticipantes, TimesPromocao, TimesRebaixamento, Divisao ) ");
                sql.Append(" VALUES ( ");
                sql.AppendFormat(" '{0}', ", campeonato.Tipo);
                sql.AppendFormat(" '{0}', ", campeonato.TemporadaId);
                sql.AppendFormat(" '{0}', ", campeonato.DataInicio.ToString("yyyy-MM-dd HH:mm:ss"));
                sql.AppendFormat(" '{0}', ", campeonato.TimesParticipantes );
                sql.AppendFormat(" '{0}', ", campeonato.TimesPromocao );
                sql.AppendFormat(" '{0}', ", campeonato.TimesRebaixamento );
                sql.AppendFormat(" '{0}' ", campeonato.Divisao );
                sql.Append(" ); ");

                return _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public Campeonato RetornarCampeonato(int campeonatoId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT * FROM campeonatos ");
                sql.AppendFormat(" where Id = '{0}' ", campeonatoId);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return PopularCampeonato(reader);
                    }
                    else
                    {
                        return new Campeonato();
                    }
                }
            }
            catch
            {
                throw;
            }            
        }

        public List<Campeonato> ListarCampeonatos(int temporadaId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT * FROM campeonatos ");
                sql.AppendFormat(" where TemporadaId = '{0}' ", temporadaId);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var campeonatos = new List<Campeonato>();
                    if (reader.Read())
                    {
                        campeonatos.Add(PopularCampeonato(reader));
                    }
                
                    return campeonatos;
                }
            }
            catch
            {
                throw;
            }
        }

        private Campeonato PopularCampeonato(SqliteDataReader reader)
        {
            try
            {
                var campeonato = new Campeonato();
                campeonato.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                campeonato.TemporadaId = reader.GetInt32(reader.GetOrdinal("TemporadaId"));
                campeonato.Tipo = reader.GetString(reader.GetOrdinal("Tipo"));
                campeonato.TimesParticipantes = reader.GetInt32(reader.GetOrdinal("TimesParticipantes"));
                campeonato.TimesPromocao = reader.GetInt32(reader.GetOrdinal("TimesPromocao"));
                campeonato.TimesRebaixamento = reader.GetInt32(reader.GetOrdinal("TimesRebaixamento"));
                campeonato.Divisao = reader.GetInt32(reader.GetOrdinal("Divisao"));

                campeonato.Rodadas = ListarRodadasPorCampeonato(campeonato.Id);

                return campeonato;
            }
            catch
            {
                throw;
            }
        }


        // ----- Campeonato Classificação ----- //
        public void InserirCampeonatoClassificacao(CampeonatoClassificacao time)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO campeonatos_classificacao ( CampeonatoId, TimeId, Jogos, Pontos, Vitoria, Empate, Derrota, GolFavor, GolContra, CartaoAmarelo, CartaoVermelho ) ");
                sql.Append(" VALUES ( ");
                sql.AppendFormat(" '{0}', ", time.CampeonatoId);
                sql.AppendFormat(" '{0}', ", time.TimeId);
                sql.Append(" '0', ");
                sql.Append(" '0', ");
                sql.Append(" '0', ");
                sql.Append(" '0', ");
                sql.Append(" '0', ");
                sql.Append(" '0', ");
                sql.Append(" '0', ");
                sql.Append(" '0', ");
                sql.Append(" '0' ");
                sql.Append(" ); ");

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public void AtualizarCampeonatoClassificacao(CampeonatoClassificacao classificacao)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE campeonatos_classificacao ");
                sql.Append(" SET ");
                sql.Append(" Jogos = Jogos + 1, ");
                sql.AppendFormat(" Pontos = Pontos + {0}, ", classificacao.Pontos);
                sql.AppendFormat(" Vitoria = Vitoria + {0}, ", classificacao.Vitoria);
                sql.AppendFormat(" Empate = Empate + {0}, ", classificacao.Empate);
                sql.AppendFormat(" Derrota = Derrota + {0}, ", classificacao.Derrota);
                sql.AppendFormat(" GolFavor = GolFavor + {0}, ", classificacao.GolFavor);
                sql.AppendFormat(" GolContra = GolContra + {0} ", classificacao.GolContra);
                sql.AppendFormat(" WHERE CampeonatoId = '{0}' ", classificacao.CampeonatoId);
                sql.AppendFormat(" AND TimeId = '{0}' ", classificacao.TimeId);

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public List<CampeonatoClassificacao> ListarCampeonatoClassificacao(int campeonatoId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT * FROM campeonatos_classificacao ");
                sql.AppendFormat(" where CampeonatoId = '{0}' ", campeonatoId);
                sql.Append(" ORDER BY Pontos desc, Vitoria desc, (GolFavor - GolContra) desc, GolFavor desc ");

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var times = new List<CampeonatoClassificacao>();

                    while (reader.Read())
                    {
                        var time = new CampeonatoClassificacao();
                        time.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                        time.TimeId = reader.GetInt32(reader.GetOrdinal("TimeId"));
                        time.Jogos = reader.GetInt32(reader.GetOrdinal("Jogos"));
                        time.Pontos = reader.GetInt32(reader.GetOrdinal("Pontos"));
                        time.Vitoria = reader.GetInt32(reader.GetOrdinal("Vitoria"));
                        time.Empate = reader.GetInt32(reader.GetOrdinal("Empate"));
                        time.Derrota = reader.GetInt32(reader.GetOrdinal("Derrota"));
                        time.GolFavor = reader.GetInt32(reader.GetOrdinal("GolFavor"));
                        time.GolContra = reader.GetInt32(reader.GetOrdinal("GolContra"));
                        time.GolSaldo = time.GolFavor - time.GolContra;
                        time.CartaoAmarelo = reader.GetInt32(reader.GetOrdinal("CartaoAmarelo"));
                        time.CartaoVermelho = reader.GetInt32(reader.GetOrdinal("CartaoVermelho"));

                        time.Time = _timeAplicacao.RetornarTime(time.TimeId);

                        times.Add(time);
                    }

                    return times;
                }
            }
            catch
            {
                throw;
            }
        }



        // ----- Campeonato Rodada ----- //
        public int InserirCampeonatoRodada(CampeonatoRodada rodada)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO campeonatos_rodadas ( CampeonatoId, Numero, Turno, Data, Concluida ) ");
                sql.Append(" VALUES ( ");
                sql.AppendFormat(" '{0}', ", rodada.CampeonatoId);
                sql.AppendFormat(" '{0}', ", rodada.Numero);
                sql.AppendFormat(" '{0}', ", rodada.Turno);
                sql.AppendFormat(" '{0}', ", rodada.Data.ToString("yyyy-MM-dd HH:mm:ss"));
                sql.AppendFormat(" '{0}' ", 0);
                sql.Append(" ); ");

                return _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public CampeonatoRodada RetornarProximaRodadaDoCampeonato(int campeonatoId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT * FROM campeonatos_rodadas ");
                sql.AppendFormat(" where CampeonatoId = '{0}' ", campeonatoId);
                sql.Append(" AND Concluida = '0' ");
                sql.Append(" ORDER BY Numero ASC ");
                sql.Append(" LIMIT 1 ");

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return PopularRodada(reader);
                    }
                    else
                    {
                        return new CampeonatoRodada();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public CampeonatoRodada RetornarProximaRodadaTemporada(int temporadaId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT t1.* ");
                sql.Append(" FROM campeonatos_rodadas t1 ");
                sql.Append(" LEFT JOIN campeonatos t2 on t2.Id = t1.CampeonatoId ");
                sql.Append(" LEFT JOIN temporadas t3 on t3.Id = t2.TemporadaId ");
                sql.AppendFormat(" WHERE t3.Id = '{0}' ", temporadaId);
                sql.Append(" AND Concluida = '0' ");
                sql.Append(" ORDER BY t1.Data ASC ");
                sql.Append(" LIMIT 1 ");

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return PopularRodada(reader);
                    }
                    else
                    {
                        return new CampeonatoRodada();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public List<CampeonatoRodada> ListarRodadasPorCampeonato(int campeonatoId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT * FROM campeonatos_rodadas ");
                sql.AppendFormat(" where CampeonatoId = '{0}' ", campeonatoId);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var times = new List<CampeonatoRodada>();

                    while (reader.Read())
                    {
                        times.Add(PopularRodada(reader));
                    }

                    return times;
                }
            }
            catch
            {
                throw;
            }
        }

        public void AtualizarConclusaoRodada(CampeonatoRodada rodada)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE campeonatos_rodadas ");
                sql.Append(" SET ");
                sql.Append(" Concluida = '1' ");
                sql.AppendFormat(" WHERE Id = '{0}' ", rodada.Id);

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        private CampeonatoRodada PopularRodada(SqliteDataReader reader)
        {
            try
            {
                var rodada = new CampeonatoRodada();
                rodada.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                rodada.CampeonatoId = reader.GetInt32(reader.GetOrdinal("CampeonatoId"));
                rodada.Numero = reader.GetInt32(reader.GetOrdinal("Numero"));
                rodada.Turno = reader.GetInt32(reader.GetOrdinal("Turno"));
                rodada.Data = reader.GetDateTime(reader.GetOrdinal("Data"));
                rodada.Concluida = reader.GetInt32(reader.GetOrdinal("Concluida")) == 1 ? true : false;

                rodada.Partidas = ListarPartidasPorRodada(rodada.Id);

                return rodada;
            }
            catch
            {
                throw;
            }
        }



        // ----- Campeonato Partida ----- //
        public int InserirCampeonatoPartida(CampeonatoPartida partida)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO campeonatos_partidas ( RodadaId, TimeCasaId, TimeVisitanteId, PlacarCasa, PlacarVisitante ) ");
                sql.Append(" VALUES ( ");
                sql.AppendFormat(" '{0}', ", partida.RodadaId);
                sql.AppendFormat(" '{0}', ", partida.TimeCasaId);
                sql.AppendFormat(" '{0}', ", partida.TimeVisitanteId);
                sql.AppendFormat(" '{0}', ", partida.PlacarCasa);
                sql.AppendFormat(" '{0}' ", partida.PlacarVisitante);
                sql.Append(" ); ");

                return _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public List<CampeonatoPartida> ListarPartidasPorRodada(int rodadaId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT * FROM campeonatos_partidas ");
                sql.AppendFormat(" where RodadaId = '{0}' ", rodadaId);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var partidas = new List<CampeonatoPartida>();

                    while (reader.Read())
                    {
                        partidas.Add(PopularPartida(reader));
                    }

                    return partidas;
                }
            }
            catch
            {
                throw;
            }
        }

        public void AtualizarPlacarDaRodada(CampeonatoPartida partida)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" UPDATE campeonatos_partidas ");
                sql.Append(" SET ");
                sql.AppendFormat(" PlacarCasa = '{0}', ", partida.PlacarCasa);
                sql.AppendFormat(" PlacarVisitante = '{0}' ", partida.PlacarVisitante);
                sql.AppendFormat(" WHERE Id = '{0}' ", partida.Id);

                _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        private CampeonatoPartida PopularPartida(SqliteDataReader reader)
        {
            try
            {
                var partida = new CampeonatoPartida();
                partida.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                partida.RodadaId = reader.GetInt32(reader.GetOrdinal("RodadaId"));
                partida.TimeCasaId = reader.GetInt32(reader.GetOrdinal("TimeCasaId"));
                partida.TimeVisitanteId = reader.GetInt32(reader.GetOrdinal("TimeVisitanteId"));
                partida.PlacarCasa = reader.GetInt32(reader.GetOrdinal("PlacarCasa"));
                partida.PlacarVisitante = reader.GetInt32(reader.GetOrdinal("PlacarVisitante"));

                partida.TimeCasa = _timeAplicacao.RetornarTime(partida.TimeCasaId);
                partida.TimeVisitante = _timeAplicacao.RetornarTime(partida.TimeVisitanteId);

                partida.Eventos = ListarCampeonatoPartidaEventos(partida.Id);

                return partida;
            }
            catch
            {
                throw;
            }
        }



        // ----- Campeonato Partida Evento ----- //
        public int InserirCampeonatoPartidaEvento(CampeonatoPartidaEvento evento)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO campeonatos_partidas_eventos ( TipoId, PartidaId, TimeId, JogadorId, Tempo ) ");
                sql.Append(" VALUES ( ");
                sql.AppendFormat(" '{0}', ", evento.TipoId);
                sql.AppendFormat(" '{0}', ", evento.PartidaId);
                sql.AppendFormat(" '{0}', ", evento.TimeId);
                sql.AppendFormat(" '{0}', ", evento.JogadorId);
                sql.AppendFormat(" '{0}' ", evento.Tempo);
                sql.Append(" ); ");

                return _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        public List<CampeonatoPartidaEvento> ListarCampeonatoPartidaEventos(int partidaId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT * FROM campeonatos_partidas_eventos ");
                sql.AppendFormat(" where PartidaId = '{0}' ", partidaId);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var eventos = new List<CampeonatoPartidaEvento>();

                    while (reader.Read())
                    {
                        eventos.Add(PopularPartidaEventos(reader));
                    }

                    return eventos;
                }
            }
            catch
            {
                throw;
            }
        }

        private CampeonatoPartidaEvento PopularPartidaEventos(SqliteDataReader reader)
        {
            try
            {
                var evento = new CampeonatoPartidaEvento();
                evento.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                evento.TipoId = reader.GetInt32(reader.GetOrdinal("TipoId"));
                evento.PartidaId = reader.GetInt32(reader.GetOrdinal("PartidaId"));
                evento.TimeId = reader.GetInt32(reader.GetOrdinal("TimeId"));
                evento.JogadorId = reader.GetInt32(reader.GetOrdinal("JogadorId"));
                evento.Tempo = reader.GetInt32(reader.GetOrdinal("Tempo"));

                evento.Jogador = _timeAplicacao.RetornarJogador(evento.JogadorId);

                return evento;
            }
            catch
            {
                throw;
            }
        }


        public List<CampeonatoArtilharia> ListarCampeonatoArtilharia(int campeonatoId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT t4.Nome as 'Jogador', t5.Nome as 'Time', count(t1.JogadorId) as 'Gols' ");
                sql.Append(" FROM campeonatos_partidas_eventos t1 ");
                sql.Append(" LEFT JOIN campeonatos_partidas t2 on t2.Id = t1.PartidaId ");
                sql.Append(" LEFT JOIN campeonatos_rodadas t3 on t3.Id = t2.RodadaId ");
                sql.Append(" LEFT JOIN jogadores t4 on t4.Id = t1.JogadorId ");
                sql.Append(" LEFT JOIN times t5 on t5.Id = t4.TimeId ");
                sql.AppendFormat(" WHERE t3.CampeonatoId = {0} and t1.TipoId = '1' ", campeonatoId);
                sql.Append(" group by t1.JogadorId ");
                sql.Append(" order by count(t1.JogadorId) desc ");

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    var artilheiros = new List<CampeonatoArtilharia>();

                    while (reader.Read())
                    {
                        var artilheiro = new CampeonatoArtilharia();
                        artilheiro.Jogador = reader.GetString(reader.GetOrdinal("Jogador"));
                        artilheiro.Time = reader.GetString(reader.GetOrdinal("Time"));
                        artilheiro.Gols = reader.GetInt32(reader.GetOrdinal("Gols"));

                        artilheiros.Add(artilheiro);
                    }

                    return artilheiros;
                }
            }
            catch
            {
                throw;
            }
        }



        // ----- Temporada ----- //
        public int RetornarTemporadaAtual()
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT Id FROM temporadas ");
                sql.Append(" WHERE Atual = 1 order by Id desc limit 1 ");

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return reader.GetInt32(reader.GetOrdinal("Id"));
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public Temporada RetornarTemporada(int temporadaId)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" SELECT * FROM temporadas ");
                sql.AppendFormat(" where Id = '{0}' ", temporadaId);

                using (var reader = _conexaoSqLite.ExecuteReader(sql.ToString()))
                {
                    if (reader.Read())
                    {
                        return PopularTemporada(reader);
                    }
                    else
                    {
                        return new Temporada();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public int InserirTemporada(Temporada temporada)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append(" INSERT INTO temporadas ( Ano, DataInicio, Atual ) ");
                sql.Append(" VALUES ( ");
                sql.AppendFormat(" '{0}', ", temporada.Ano);
                sql.AppendFormat(" '{0}', ", temporada.DataInicio.ToString("yyyy-MM-dd HH:mm:ss"));
                sql.AppendFormat(" '{0}' ", temporada.Atual ? 1 : 0);
                sql.Append(" ); ");

                return _conexaoSqLite.ExecuteNonQuery(sql.ToString());
            }
            catch
            {
                throw;
            }
        }

        private Temporada PopularTemporada(SqliteDataReader reader)
        {
            try
            {
                var temporada = new Temporada();
                temporada.Id = reader.GetInt32(reader.GetOrdinal("Id"));
                temporada.Ano = reader.GetInt32(reader.GetOrdinal("Ano"));
                temporada.DataInicio = reader[reader.GetOrdinal("DataInicio")] != DBNull.Value ? reader.GetDateTime("DataInicio") : new DateTime();
                temporada.Atual = reader[reader.GetOrdinal("Atual")] != DBNull.Value ? reader.GetBoolean("Atual") : false;

                temporada.Campeonatos = ListarCampeonatos(temporada.Id);

                return temporada;
            }
            catch
            {
                throw;
            }
        }

    }
}
