using MFoot.Maui.Aplicacao;
using MFoot.Maui.Domain;
using MFoot.Maui.Models;
using System.Collections.ObjectModel;

namespace MFoot.Maui.Views.Partidas;

public partial class SimularPartidas : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CampeonatoAplicacao _campeonatoAplicacao;
    private readonly TimeAplicacao _timeAplicacao;
    private readonly GameAplicacao _gameAplicacao;

    private ObservableCollection<CampeonatoPartida> partidas;
    private ObservableCollection<CampeonatoPartida> partidasPrimeiraDivisao;
    private ObservableCollection<CampeonatoPartida> partidasSegundaDivisao;
    private int updateCount;
    private CampeonatoRodada _rodadaPrimeiraDivisao;
    private CampeonatoRodada _rodadaSegundaDivisao;

    public SimularPartidas(IServiceProvider serviceProvider, CampeonatoAplicacao campeonatoAplicacao, TimeAplicacao timeAplicacao, GameAplicacao gameAplicacao)
	{
		InitializeComponent();

        _serviceProvider = serviceProvider;
        _campeonatoAplicacao = campeonatoAplicacao;
        _timeAplicacao = timeAplicacao;
        _gameAplicacao = gameAplicacao;

        CarregarPartidas();
        AtualizarPlacarPeriodicamente();
    }

    private void CarregarPartidas()
    {
        _rodadaPrimeiraDivisao = _campeonatoAplicacao.RetornarProximaRodadaDoCampeonato(1);
        _rodadaSegundaDivisao = _campeonatoAplicacao.RetornarProximaRodadaDoCampeonato(2);

        partidasPrimeiraDivisao = new ObservableCollection<CampeonatoPartida>(_rodadaPrimeiraDivisao.Partidas);
        partidasSegundaDivisao = new ObservableCollection<CampeonatoPartida>(_rodadaSegundaDivisao.Partidas);

        partidas = new ObservableCollection<CampeonatoPartida>(partidasPrimeiraDivisao.Concat(partidasSegundaDivisao));

        partidasPrimeiraDivisaoView.ItemsSource = partidasPrimeiraDivisao;
        partidasSegundaDivisaoView.ItemsSource = partidasSegundaDivisao;
    }
   
    private double RetornarChanceGol(Time time1, Time time2, bool mandante)
    {
        try
        {
            var ataque = time1.JogadoresTitulares.Sum(x => x.Ataque) / 11;
            var defesa = time2.JogadoresTitulares.Sum(x => x.Defesa) / 11;

            //var probabilidade = (ataque + (mandante ? 200 : 100)) - defesa;
            var probabilidade = (ataque + (mandante ? 20 : 15)) - defesa;

            return probabilidade >= 12 ? Math.Round(Convert.ToDouble(probabilidade) / 10, 2) : 1.2;
        }
        catch
        {
            throw;
        }
    }

    private async void AtualizarPlacarPeriodicamente()
    {
        try
        {
            while (updateCount <= 90)
            {
                Tempo.Text = updateCount.ToString("00");

                var validaResistencia = (updateCount > 0) & (updateCount % 15 == 0);

                foreach (var partida in partidas)
                {
                    var chanceCasa = RetornarChanceGol(partida.TimeCasa, partida.TimeVisitante, true);
                    var chanceVistante = RetornarChanceGol(partida.TimeVisitante, partida.TimeCasa, false);

                    var eventoProcessado = ProcessarEvento(chanceCasa, chanceVistante);
                    var eventoExibicao = "";

                    if (eventoProcessado != "")
                    {
                        var evento = new CampeonatoPartidaEvento();
                        evento.PartidaId = partida.Id;

                        if (eventoProcessado == "gol_casa")
                        {
                            var jogadorGolCasa = RetonarJogadorMarcarGol(partida.TimeCasa);

                            // Atualiza placar da partida
                            partida.PlacarCasa += 1;
                            partida.EventoId = 1;

                            // Atualiza dados do evento
                            evento.TipoId = 1;
                            evento.TimeId = partida.TimeCasaId;
                            evento.JogadorId = jogadorGolCasa.Id;
                            evento.Tempo = updateCount;

                            eventoExibicao = "Gol ( " + jogadorGolCasa.Nome + " )";
                        }

                        if (eventoProcessado == "gol_visitante")
                        {
                            var jogadorGolVisitante = RetonarJogadorMarcarGol(partida.TimeVisitante);

                            // Atualiza placar da partida
                            partida.PlacarVisitante += 1;
                            partida.EventoId = 1;

                            // Atualiza dados do evento
                            evento.TipoId = 1;
                            evento.TimeId = partida.TimeVisitanteId;
                            evento.JogadorId = jogadorGolVisitante.Id;
                            evento.Tempo = updateCount;

                            eventoExibicao = "Gol ( " + RetonarJogadorMarcarGol(partida.TimeVisitante).Nome + " )";
                        }

                        if (eventoProcessado == "cartao_amarelo")
                        {
                            var jogadorCartaoAmarelo = RetonarJogadorCartaoAmarelo(partida.TimeCasa, partida.TimeVisitante);

                            partida.EventoId = 2;

                            // Atualiza dados do evento
                            evento.TipoId = 2;
                            evento.TimeId = jogadorCartaoAmarelo.TimeId;
                            evento.JogadorId = jogadorCartaoAmarelo.Id;
                            evento.Tempo = updateCount;

                            eventoExibicao = "Cartão amarelo ( " + jogadorCartaoAmarelo.Nome + " )";
                        }                        

                        if (eventoProcessado == "cartao_vermelho")
                        {
                            var jogadorCartaoVermelho = RetonarJogadorCartaoVermelho(partida.TimeCasa, partida.TimeVisitante);

                            partida.EventoId = 3;

                            // Atualiza dados do evento
                            evento.TipoId = 3;
                            evento.TimeId = jogadorCartaoVermelho.TimeId;
                            evento.JogadorId = jogadorCartaoVermelho.Id;
                            evento.Tempo = updateCount;

                            eventoExibicao = "Cartão vermelho ( " + jogadorCartaoVermelho.Nome + " )";

                            RemoverJogadorPorExpulsao(jogadorCartaoVermelho, partida.TimeCasa, partida.TimeVisitante);
                        }

                        partida.Eventos.Add(evento);
                        partida.Evento = eventoExibicao;

                        var segundoCartao = partida.Eventos.Where(x => x.JogadorId == evento.JogadorId && x.TipoId == 2).Count();

                        if (segundoCartao == 2)
                        {  
                            var jogadorSegundoCartao = RetonarJogador(evento.JogadorId, partida.TimeCasa, partida.TimeVisitante);

                            partida.EventoId = 3;

                            var novoEvento = new CampeonatoPartidaEvento
                            {
                                PartidaId = partida.Id,
                                TipoId = 3,
                                TimeId = jogadorSegundoCartao.TimeId,
                                JogadorId = jogadorSegundoCartao.Id,
                                Tempo = updateCount
                            };

                            eventoExibicao = "Cartão vermelho ( " + jogadorSegundoCartao.Nome + " )";

                            RemoverJogadorPorExpulsao(jogadorSegundoCartao, partida.TimeCasa, partida.TimeVisitante);

                            partida.Eventos.Add(novoEvento);
                            partida.Evento = eventoExibicao;
                        }
                        
                    }

                    if (validaResistencia)
                    {
                        foreach (var jogador in partida.TimeCasa.JogadoresTitulares)
                        {
                            jogador.Resistencia = CalculaResistenciaPerdida(jogador);
                        }

                        foreach (var jogador in partida.TimeVisitante.JogadoresTitulares)
                        {
                            jogador.Resistencia = CalculaResistenciaPerdida(jogador); ;
                        }
                    }


                    // ALterar a resitencia dos jogadore

                }

                updateCount++;
                await Task.Delay(100); // Espera 1 segundo
            }

            // Persistir os dados
            _gameAplicacao.AtualizarDadosDaRodada(_rodadaPrimeiraDivisao);
            _gameAplicacao.AtualizarDadosDaRodada(_rodadaSegundaDivisao);

            // Atualizar dados Pós Rodada
            _timeAplicacao.RecuperarResistenciaJogadoresPosRodada();
            _timeAplicacao.SubstituirJogadoresResistenciaBaixa();
            _timeAplicacao.SubstituirJogadoresSuspensos();

            Application.Current.MainPage = _serviceProvider.GetService<HomePage>();
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", $"Houve um erro ao processar a rotina: {ex.Message}", "OK");
        }
    }

    private double CalculaResistenciaPerdida(Jogador jogador)
    {
        if (jogador.Resistencia == 0.00)
            return 0.00;

        double resistenciaPerdida = (jogador.Idade / 60.00) * 15;
        
        double resistencia = Math.Round(jogador.Resistencia - resistenciaPerdida, 2);

        if (resistencia < 0.00)
            return 0.00;

        return resistencia;
    }

    private Jogador RetonarJogadorMarcarGol(Time time)
    {
        var jogadores = time.JogadoresTitulares;

        Dictionary<double[], Jogador> ranges = new Dictionary<double[], Jogador>();

        double rangeInicio = 0.00;
        double rangeFim = 0.00;

        foreach (var jogador in jogadores)
        {
            rangeFim = rangeInicio + Math.Round((jogador.Ataque + jogador.Finalizacao) * RetornarEqualizadorPosicao(jogador.Posicao));

            double[] range = new double[] { rangeInicio, rangeFim };

            ranges.Add(range, jogador);

            rangeInicio = rangeFim;
        }

        var random = new Random();

        // Gera um número aleatório do tipo double entre 0.0 e 100.0
        double randomValue = Math.Round(random.NextDouble() * rangeFim);

        Jogador retorno = new Jogador();

        // Verifica em qual intervalo o número caiu usando o Dictionary
        foreach (var range in ranges)
        {
            if (randomValue > range.Key[0] && randomValue <= range.Key[1])
            {
                retorno = range.Value;
                break;
            }
        }

        return retorno;
    }

    private Jogador RetonarJogadorCartaoAmarelo(Time timeCasa, Time timeVisitante)
    {
        var jogadoresCasa = timeCasa.JogadoresTitulares;
        var jogadoresVistante = timeVisitante.JogadoresTitulares;

        var jogadores = new List<Jogador>();
        jogadores.AddRange(jogadoresCasa);
        jogadores.AddRange(jogadoresVistante);

        // Gera um índice aleatório e retorna o jogador correspondente
        var random = new Random();
        int index = random.Next(jogadores.Count);
        return jogadores[index];
    }

    private Jogador RetonarJogadorCartaoVermelho(Time timeCasa, Time timeVisitante)
    {
        var jogadoresCasa = timeCasa.JogadoresTitulares;
        var jogadoresVistante = timeVisitante.JogadoresTitulares;

        var jogadores = new List<Jogador>();
        jogadores.AddRange(jogadoresCasa);
        jogadores.AddRange(jogadoresVistante);

        // Gera um índice aleatório e retorna o jogador correspondente
        var random = new Random();
        int index = random.Next(jogadores.Count);
        return jogadores[index];
    }

    private void RemoverJogadorPorExpulsao(Jogador jogador, Time timeCasa, Time timeVisitante)
    {
        if (timeCasa.JogadoresTitulares.Contains(jogador))
        {
            // Se o jogador pertence ao time da casa
            jogador.Titular = false;
            timeCasa.JogadoresTitulares.Remove(jogador); // Remove o jogador da lista de titulares
        }
        else if (timeVisitante.JogadoresTitulares.Contains(jogador))
        {
            // Se o jogador pertence ao time visitante
            jogador.Titular = false;
            timeVisitante.JogadoresTitulares.Remove(jogador); // Remove o jogador da lista de titulares
        }
    }

    private Jogador RetonarJogador(int jogadorId, Time timeCasa, Time timeVisitante)
    {
        var jogadoresCasa = timeCasa.JogadoresTitulares;
        var jogadoresVistante = timeVisitante.JogadoresTitulares;

        var jogadores = new List<Jogador>();
        jogadores.AddRange(jogadoresCasa);
        jogadores.AddRange(jogadoresVistante);

        return jogadores.Where(x => x.Id == jogadorId).FirstOrDefault();
    }

    private double RetornarEqualizadorPosicao(string zona)
    {
        Dictionary<string, double> valores = new Dictionary<string, double>
        {
            { "GOL", 0.01 },
            { "ZAG", 0.40 },
            { "LD", 0.40 },
            { "LE", 0.40 },
            { "VOL", 0.70 },
            { "ME", 1.00 },
            { "SA", 1.40 },
            { "CA", 1.60 }
        };

        return valores[zona];
    }

    private string ProcessarEvento(double chanceCasa, double chanceVistante)
    {
        var range1 = 0.00;
        var range2 = range1 + chanceCasa;
        var range3 = range2 + chanceVistante;
        var range4 = range3 + 5.00;
        var range5 = range4 + 0.15;
        var range6 = 100.00;

        double[] rangeGolCasa = new double[] { range1, range2 };
        double[] rangeGolVisitante = new double[] { range2, range3 };
        double[] rangeCartaoAmarelo = new double[] { range3, range4 };
        double[] rangeCartaoVermelho = new double[] { range4, range5 };
        double[] nenhumEvento = new double[] { range5, range6 };


        Dictionary<double[], string> ranges = new Dictionary<double[], string>()
        {
            { rangeGolCasa, "gol_casa" },
            { rangeGolVisitante, "gol_visitante" },
            { rangeCartaoAmarelo, "cartao_amarelo" },
            { rangeCartaoVermelho, "cartao_vermelho" },
            { nenhumEvento, "" }
        };

        var random = new Random();

        // Gera um número aleatório do tipo double entre 0.0 e 100.0
        double randomValue = random.NextDouble() * 100;

        var retorno = "";

        // Verifica em qual intervalo o número caiu usando o Dictionary
        foreach (var range in ranges)
        {
            if (randomValue > range.Key[0] && randomValue <= range.Key[1])
            {
                retorno = range.Value;
                break;
            }
        }

        return retorno;
    }

}