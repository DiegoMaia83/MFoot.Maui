using MFoot.Maui.Dal;
using MFoot.Maui.Views;

namespace MFoot.Maui;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();

        Preferences.Remove("currentGameDatabase");

        MainPage = serviceProvider.GetService<InicioPage>();
        //MainPage = new MainPage();
    }
}
