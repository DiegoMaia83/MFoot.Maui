using MFoot.Maui.Configuration;

namespace MFoot.Maui.Views.Temporada;

public partial class AvancarDiasPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;

    public AvancarDiasPage(IServiceProvider serviceProvider)
	{
		InitializeComponent();

        _serviceProvider = serviceProvider;

        labelTemporadaAno.Text = $"Temporada {GameConfiguration.AnoAtual}";
        labelTemporadaDataAtual.Text = Convert.ToDateTime(GameConfiguration.DataAtual).ToLongDateString();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = _serviceProvider.GetService<AvancarDiasPage>();
    }

    private void Button_AvancarDias(object sender, EventArgs e)
    {
        var dataAtual = Convert.ToDateTime(GameConfiguration.DataAtual);

        var proximoDia = dataAtual.AddDays(1);

        GameConfiguration.DataAtual = proximoDia.ToString("yyyy-MM-dd");




        Application.Current.MainPage = _serviceProvider.GetService<HomePage>();
    }
}