using MFoot.Maui.Aplicacao;
using MFoot.Maui.Builder.Views;
using MFoot.Maui.Configuration;
using MFoot.Maui.Domain;
using MFoot.Maui.Models;
using MFoot.Maui.Views.Partidas;
using MFoot.Maui.Views.Temporada;

namespace MFoot.Maui.Views;

public partial class HomePage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CampeonatoAplicacao _campeonatoAplicacao;
    private readonly TimeAplicacao _timeAplicacao;

	public HomePage(IServiceProvider serviceProvider, CampeonatoAplicacao campeonatoAplicacao, TimeAplicacao timeAplicacao)
	{
		InitializeComponent();

        _serviceProvider = serviceProvider;
        _campeonatoAplicacao = campeonatoAplicacao;
        _timeAplicacao = timeAplicacao;

        // Retorna os dados da temporada atual
        var temporada = _campeonatoAplicacao.RetornarTemporada(Convert.ToInt32(GameConfiguration.TemporadaAtual));

        // Retorna a próxima rodada
        var proximaRodada = _campeonatoAplicacao.RetornarProximaRodadaTemporada(Convert.ToInt32(GameConfiguration.TemporadaAtual));

        if (proximaRodada.Data.ToString("yyyy-MM-dd") == GameConfiguration.DataAtual)
        {
            btnProximaRodada.IsVisible = true;
        }
        else
        {
            btnAvancarDias.IsVisible = true;
        }

        labelTemporadaAno.Text = $"Temporada {GameConfiguration.AnoAtual}";
        labelTemporadaDataAtual.Text = Convert.ToDateTime(GameConfiguration.DataAtual).ToLongDateString();

        pickerDivisao.SelectedIndex = 0;

        var times = _timeAplicacao.ListarTimes().OrderByDescending(x => x.Ataque + x.Defesa).ToList();
        var timesGrid = CampeonatoViewBuilder.BuildTimesLayout(times);
        timesStackLayout.Children.Add(timesGrid);
    }

    private void Button_ProximaRodada(object sender, EventArgs e)
    {
        Application.Current.MainPage = _serviceProvider.GetService<SimularPartidas>();
    }

    private void Button_AvancarDias(object sender, EventArgs e)
    {
        Application.Current.MainPage = _serviceProvider.GetService<AvancarDiasPage>();
    }

    private void Picker_SelecionarDivisao(object sender, EventArgs e)
    {
        // Convertendo o sender para Picker
        Picker picker = sender as Picker;

        // Verificando se o picker não é nulo e se algo foi selecionado
        if (picker != null && picker.SelectedIndex != -1)
        {
            var id = picker.SelectedIndex + 1;

            if (id > 0)
            {
                // Limpar o conteúdo existente
                classificacaoStackLayout.Children.Clear();
                rodadasStackLayout.Children.Clear();
                artilhariaStackLayout.Children.Clear();

                var classificacao = _campeonatoAplicacao.ListarCampeonatoClassificacao(id);
                var classificacaoGrid = CampeonatoViewBuilder.BuildClassificacaoLayout(classificacao);
                classificacaoStackLayout.Children.Add(classificacaoGrid);

                var campeonato = _campeonatoAplicacao.RetornarCampeonato(id);
                var rodadasGrid = CampeonatoViewBuilder.BuildRodadasGrid(campeonato.Rodadas, Button_AbrirModalPartidaDetalhes);
                rodadasStackLayout.Children.Add(rodadasGrid);

                var artilharia = _campeonatoAplicacao.ListarCampeonatoArtilharia(id);
                var artilheirosGrid = CampeonatoViewBuilder.BuildArtilheirosLayout(artilharia);
                artilhariaStackLayout.Children.Add(artilheirosGrid);
            }
        }
    }

    private void Button_AbrirModalPartidaDetalhes(CampeonatoPartida partida)
    {
        detalhesPartidaStackLayout.Clear();

        var detalhesPartidaGrid = CampeonatoViewBuilder.BuildPartidaDetalhesGrid(partida);
        detalhesPartidaStackLayout.Children.Add(detalhesPartidaGrid);

        modalOverlay.IsVisible = true;
    }

    private void Button_FecharModalPartidaDetalhes(object sender, EventArgs e)
    {
        modalOverlay.IsVisible = false;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = _serviceProvider.GetService<HomePage>();
    }
}