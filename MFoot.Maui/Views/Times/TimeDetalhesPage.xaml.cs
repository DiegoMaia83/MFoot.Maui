using MFoot.Maui.Builder.Views;
using MFoot.Maui.Domain;

namespace MFoot.Maui.Views.Times;

public partial class TimeDetalhesPage : ContentPage
{
	public TimeDetalhesPage(Time time)
	{
		InitializeComponent();

        var jogadoresTitularesGrid = CampeonatoViewBuilder.BuildJogadoresTimeGrid(time.JogadoresTitulares);
        jogadoresTitularesStackLayout.Children.Add(jogadoresTitularesGrid);

        var jogadoresReservasGrid = CampeonatoViewBuilder.BuildJogadoresTimeGrid(time.JogadoresReservas);
        jogadoresReservasStackLayout.Children.Add(jogadoresReservasGrid);
    }

    private async void Fechar_Clicked(object sender, EventArgs e)
    {
        // Fecha o modal
        await Navigation.PopModalAsync();
    }
}