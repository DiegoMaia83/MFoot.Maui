namespace MFoot.Maui.Views.Partidas;

public partial class PartidaDetalhesPage : ContentPage
{
	public PartidaDetalhesPage()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage.Navigation.PopModalAsync();
    }
}