using MFoot.Maui.Aplicacao;
using MFoot.Maui.Configuration;
using MFoot.Maui.Dal;
using MFoot.Maui.Domain;
using System.Net.Security;

namespace MFoot.Maui.Views;

public partial class InicioPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    private readonly GameAplicacao _gameAplicacao;

    public InicioPage(IServiceProvider serviceProvider, GameAplicacao gameAplicacao)
	{
        InitializeComponent();

        _serviceProvider = serviceProvider;
        _gameAplicacao = gameAplicacao;

        try
        {  
            string dbDirectory = DbConfiguration.PathtDb;
            var jogosSalvos = new List<string>();

            if (Directory.Exists(dbDirectory))
            {
                var files = Directory.GetFiles(dbDirectory);

                foreach (var file in files)
                {
                    jogosSalvos.Add(Path.GetFileName(file));
                }
            }
            else
            {
                Directory.CreateDirectory(dbDirectory);
            }

            jogosSalvosListView.ItemsSource = jogosSalvos;
        }
        catch (Exception ex)
        {
            DisplayAlert("Erro", $"Ocorreu um erro ao carregar os jogos salvos: {ex.Message}", "OK");
        }
    }

    private async void Button_NovoJogo(object sender, EventArgs e)
    {
        ShowAlert("Criando os dados do novo jogo...");
        await Task.Delay(500);

        var currentDb = $"game_{DateTime.Now.ToString("dd-MM-yy_HH-mm-ss")}.db";
        DbConfiguration.CurrentDb = currentDb;

        await _gameAplicacao.IniciarJogo(async (message) =>
        {
            ShowAlert(message);
            await Task.Delay(500); // Atraso para que a mensagem seja visível antes de continuar
        });

        Application.Current.MainPage = _serviceProvider.GetService<HomePage>();
    }

    private async void Button_ContinuarJogo(object sender, EventArgs e)
    {
        ShowAlert("Carregando os dados do jogo...");
        await Task.Delay(500);

        var button = sender as Button;
        if (button != null)
        {
            string jogoSalvo = button.Text;
            DbConfiguration.CurrentDb = jogoSalvo;

            Application.Current.MainPage = _serviceProvider.GetService<HomePage>();
        }
    }

    private void ShowAlert(string message)
    {
        if (!String.IsNullOrEmpty(message))
            alertBoxMessage.Text = message;

        alertBoxVisible.IsVisible = true;
    }

    private void HideAlert()
    {
        alertBoxVisible.IsVisible = false;
    }
}