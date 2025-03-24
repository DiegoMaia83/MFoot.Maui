using MFoot.Maui.Configuration;
using MFoot.Maui.Domain;
using MFoot.Maui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFoot.Maui.Aplicacao
{
    public class GameAplicacao
    {
        private readonly BaseAplicacao _baseAplicacao;
        private readonly CampeonatoAplicacao _campeonatoAplicacao;
        private readonly TimeAplicacao _timeAplicacao;
        private readonly DatabaseAplicacao _databaseAplicacao;

        public GameAplicacao(CampeonatoAplicacao campeonatoAplicacao, TimeAplicacao timeAplicacao, DatabaseAplicacao databaseAplicacao, BaseAplicacao baseAplicacao)
        {
            _campeonatoAplicacao = campeonatoAplicacao;
            _timeAplicacao = timeAplicacao;
            _databaseAplicacao = databaseAplicacao;
            _baseAplicacao = baseAplicacao;
        }

        public async Task IniciarJogo(Func<string, Task> updateStatus)
        {
            try
            {
                GameConfiguration.AnoAtual = "2025";
                GameConfiguration.DataAtual = "2025-03-15";
                GameConfiguration.TemporadaAtual = "1";

                int intervaloEntrePartidas = 3;

                await updateStatus("Criando banco de dados...");
                InserirTabelas();

                await updateStatus("Criando times...");
                var timesBase = _baseAplicacao.ListarTimesBase();

                foreach (var time in timesBase)
                {
                    _timeAplicacao.InserirTime(time);
                }

                await updateStatus("Criando jogadores...");
                var jogadoresBase = _baseAplicacao.ListarJogadoresBase();

                foreach (var jogador in jogadoresBase)
                {
                    _timeAplicacao.InserirJogador(jogador);
                }

                await updateStatus("Criando nova temporada...");
                // Inserir temporada inicial
                var temporada = new Temporada
                {
                    Ano = Convert.ToInt32(GameConfiguration.TemporadaAtual),
                    DataInicio = Convert.ToDateTime(GameConfiguration.DataAtual),
                    Atual = true
                };

                var temporadaInseridaId = _campeonatoAplicacao.InserirTemporada(temporada);

                var dataAtual = Convert.ToDateTime(GameConfiguration.DataAtual);


                await updateStatus("Criando campeonatos...");
                var divisoes = 2;

                for (var divisao = 1; divisao <= divisoes; divisao++)
                {
                    // Inserir temporada inicial
                    var campeonato = new Campeonato
                    {
                        Tipo = "Brasileiro",
                        TemporadaId = temporadaInseridaId,
                        DataInicio = dataAtual,
                        TimesParticipantes = 20,
                        TimesPromocao = 4,
                        TimesRebaixamento = 4,
                        Divisao = divisao
                    };

                    var campeonatoInseridoId = _campeonatoAplicacao.InserirCampeonato(campeonato);

                    var timesDivisao = _timeAplicacao.ListarTimesPorDivisao(divisao);

                    foreach (var time in timesDivisao)
                    {
                        var campeonatoTime = new CampeonatoClassificacao { CampeonatoId = campeonatoInseridoId, TimeId = time.Id };

                        _campeonatoAplicacao.InserirCampeonatoClassificacao(campeonatoTime);
                    }

                    await updateStatus("Gerando partidas...");

                    var listarTimes = _campeonatoAplicacao.ListarCampeonatoClassificacao(campeonatoInseridoId);
                    var times = listarTimes.ToList();

                    var rodadasPorTurno = timesDivisao.Count() - 1;
                    
                    var turnos = 2;

                    for (var turno = 1; turno <= turnos; turno++)
                    {
                        for (var i = 1; i <= rodadasPorTurno; i++)
                        {
                            dataAtual = dataAtual.AddDays(intervaloEntrePartidas);

                            var rodada = new CampeonatoRodada
                            {
                                CampeonatoId = campeonatoInseridoId,
                                Numero = turno == 2 ? i + times.Count - 1 : i,
                                Data = dataAtual,
                                Turno = turno
                            };

                            _campeonatoAplicacao.InserirCampeonatoRodada(rodada);
                        }
                    }

                    var listarRodadas = _campeonatoAplicacao.ListarRodadasPorCampeonato(campeonatoInseridoId);                    

                    var rodadasPrimeiroTurno = listarRodadas.Where(x => x.Turno == 1).OrderBy(x => x.Numero);                    
                    var rodadasSegundoTurno = listarRodadas.Where(x => x.Turno == 2).OrderBy(x => x.Numero);                    

                    int qtdRodadas = times.Count - 1;

                    // Cria as partidas do primeiro turno
                    for (int rodadaIndex = 0; rodadaIndex < qtdRodadas; rodadaIndex++)
                    {
                        var rodadaId = rodadasPrimeiroTurno.Where(x => x.Numero == rodadaIndex + 1).Select(x => x.Id).FirstOrDefault();

                        for (int i = 0; i < times.Count / 2; i++)
                        {
                            int timeCasaIndex = (rodadaIndex + i) % (times.Count - 1);
                            int timeForaIndex = (times.Count - 1 - i + rodadaIndex) % (times.Count - 1);

                            if (i == 0)
                            {
                                timeForaIndex = times.Count - 1;
                            }

                            var timeCasa = times[timeCasaIndex];
                            var timeFora = times[timeForaIndex];

                            // Alterna o time de casa e fora a cada rodada ímpar
                            if (rodadaIndex % 2 == 1)
                            {
                                var temp = timeCasa;
                                timeCasa = timeFora;
                                timeFora = temp;
                            }

                            var p = new CampeonatoPartida() { RodadaId = rodadaId, TimeCasaId = timeCasa.TimeId, TimeVisitanteId = timeFora.TimeId };
                            _campeonatoAplicacao.InserirCampeonatoPartida(p);

                        }
                    }

                    // Cria as partidas do segundo turno
                    for (int rodadaIndex = times.Count - 1; rodadaIndex < (qtdRodadas * 2); rodadaIndex++)
                    {
                        var rodadaId = rodadasSegundoTurno.Where(x => x.Numero == rodadaIndex + 1).Select(x => x.Id).FirstOrDefault();

                        for (int i = 0; i < times.Count / 2; i++)
                        {
                            int timeCasaIndex = (rodadaIndex + i) % (times.Count - 1);
                            int timeForaIndex = (times.Count - 1 - i + rodadaIndex) % (times.Count - 1);

                            if (i == 0)
                            {
                                timeForaIndex = times.Count - 1;
                            }

                            var timeCasa = times[timeCasaIndex];
                            var timeFora = times[timeForaIndex];

                            // Alterna o time de casa e fora a cada rodada ímpar
                            if (rodadaIndex % 2 == 1)
                            {
                                var temp = timeCasa;
                                timeCasa = timeFora;
                                timeFora = temp;
                            }

                            var p = new CampeonatoPartida() { RodadaId = rodadaId, TimeCasaId = timeCasa.TimeId, TimeVisitanteId = timeFora.TimeId };
                            _campeonatoAplicacao.InserirCampeonatoPartida(p);

                        }
                    }
                }

                await updateStatus("Iniciando jogo...");
                await Task.Delay(2000);
            }
            catch
            {
                throw;
            }
        }

        public void AtualizarDadosDaRodada(CampeonatoRodada rodada)
        {
            try
            {
                _campeonatoAplicacao.AtualizarConclusaoRodada(rodada);

                foreach (var partida in rodada.Partidas)
                {
                    _campeonatoAplicacao.AtualizarPlacarDaRodada(partida);

                    var classificacaoCasa = new CampeonatoClassificacao();
                    classificacaoCasa.CampeonatoId = rodada.CampeonatoId;
                    classificacaoCasa.TimeId = partida.TimeCasaId;
                    classificacaoCasa.Pontos = (partida.PlacarCasa > partida.PlacarVisitante ? 3 : (partida.PlacarCasa == partida.PlacarVisitante ? 1 : 0));
                    classificacaoCasa.Vitoria = partida.PlacarCasa > partida.PlacarVisitante ? 1 : 0;
                    classificacaoCasa.Empate = partida.PlacarCasa == partida.PlacarVisitante ? 1 : 0;
                    classificacaoCasa.Derrota = partida.PlacarCasa < partida.PlacarVisitante ? 1 : 0;
                    classificacaoCasa.GolFavor = partida.PlacarCasa;
                    classificacaoCasa.GolContra = partida.PlacarVisitante;

                    _campeonatoAplicacao.AtualizarCampeonatoClassificacao(classificacaoCasa);                    

                    var classificacaoVisitante = new CampeonatoClassificacao();
                    classificacaoVisitante.CampeonatoId = rodada.CampeonatoId;
                    classificacaoVisitante.TimeId = partida.TimeVisitanteId;
                    classificacaoVisitante.Pontos = (partida.PlacarVisitante > partida.PlacarCasa ? 3 : (partida.PlacarVisitante == partida.PlacarCasa ? 1 : 0));
                    classificacaoVisitante.Vitoria = partida.PlacarVisitante > partida.PlacarCasa ? 1 : 0;
                    classificacaoVisitante.Empate = partida.PlacarVisitante == partida.PlacarCasa ? 1 : 0;
                    classificacaoVisitante.Derrota = partida.PlacarVisitante < partida.PlacarCasa ? 1 : 0;
                    classificacaoVisitante.GolFavor = partida.PlacarVisitante;
                    classificacaoVisitante.GolContra = partida.PlacarCasa;

                    _campeonatoAplicacao.AtualizarCampeonatoClassificacao(classificacaoVisitante);

                    foreach (var evento in partida.Eventos)
                    {
                        _campeonatoAplicacao.InserirCampeonatoPartidaEvento(evento);

                        if (evento.TipoId == 2)
                        {
                            var cartao = new JogadorCartao
                            {
                                TipoId = 2,
                                CampeonatoId = rodada.CampeonatoId,
                                TimeId = evento.TimeId,
                                JogadorId = evento.JogadorId,
                                Concluido = false
                            };

                            _timeAplicacao.InserirJogadorCartao(cartao);
                        }

                        if (evento.TipoId == 3)
                        {
                            var cartao = new JogadorCartao
                            {
                                TipoId = 3,
                                CampeonatoId = rodada.CampeonatoId,
                                TimeId = evento.TimeId,
                                JogadorId = evento.JogadorId,
                                Concluido = false
                            };

                            _timeAplicacao.InserirJogadorCartao(cartao);
                            _timeAplicacao.RemoveJogadorCartaoAmarelo(cartao);


                            var suspensao = new JogadorSuspensao
                            {                                
                                CampeonatoId = rodada.CampeonatoId,
                                TimeId = evento.TimeId,
                                JogadorId = evento.JogadorId,
                                QuantidadeJogos = 1,
                                Concluido = false
                            };

                            _timeAplicacao.InserirJogadorSuspensao(suspensao);
                        }                        
                    }

                    if (classificacaoCasa.Vitoria == 1)
                        _timeAplicacao.AtualizaJogadoresVitoria(classificacaoCasa.TimeId);

                    if (classificacaoCasa.Derrota == 1)
                        _timeAplicacao.AtualizaJogadoresDerrota(classificacaoCasa.TimeId);

                    if (classificacaoVisitante.Vitoria == 1)
                        _timeAplicacao.AtualizaJogadoresVitoria(classificacaoVisitante.TimeId);

                    if (classificacaoVisitante.Derrota == 1)
                        _timeAplicacao.AtualizaJogadoresDerrota(classificacaoVisitante.TimeId);

                    foreach (var jogador in partida.TimeCasa.Jogadores)
                    {
                        _timeAplicacao.AtualizaDadosJogador(jogador);
                    }

                    foreach (var jogador in partida.TimeVisitante.Jogadores)
                    {
                        _timeAplicacao.AtualizaDadosJogador(jogador);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void InserirTabelas()
        {
            try
            {
                // Inserir tabela das temporadas
                _databaseAplicacao.InserirTabelaTemporada();

                // Inserir tabela dos campeonatos
                _databaseAplicacao.InserirTabelaCampeonato();

                // Inserir tabela de classificacoes dos campeonatos
                _databaseAplicacao.InserirTabelaCampeonatoClassificacao();

                // Inserir tabela das rodadas dos campeonatos
                _databaseAplicacao.InserirTabelaCampeonatoRodada();

                // Inserir tabela das partidas dos campeonatos
                _databaseAplicacao.InserirTabelaCampeonatoPartida();

                // Inserir tabela dos eventos das partidas
                _databaseAplicacao.InserirTabelaCampeonatoPartidaEvento();                
                
                // Inserir tabela dos eventos das partidas
                _databaseAplicacao.InserirTabelaCampeonatoPartidaEventoTipo();

                // Inserir tabelas dos times
                _databaseAplicacao.InserirTabelaTime();

                // Inserir tabelas dos jogadores
                _databaseAplicacao.InserirTabelaJogador();

                // Inserir tabelas dos cartões dos jogadores
                _databaseAplicacao.InserirTabelaJogadorCartoes();

                // Inserir tabelas das suspensões dos jogadores
                _databaseAplicacao.InserirTabelaJogadorSuspensoes();

                // Insere os tipos de eventos na tabela
                _databaseAplicacao.InserirCampeonatoPartidaEventoTipo();
            }
            catch
            {
                throw;
            }
        }
    }
}
