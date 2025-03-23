using MFoot.Maui.Aplicacao;
using MFoot.Maui.Configuration;
using MFoot.Maui.Dal;
using MFoot.Maui.Views;
using MFoot.Maui.Views.Partidas;
using MFoot.Maui.Views.Temporada;
using Microsoft.Extensions.Logging;

namespace MFoot.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        // Registra ConexaoSqLite com os parâmetros necessários
        builder.Services.AddScoped<ConexaoSqLite>();

        builder.Services.AddSingleton<CampeonatoAplicacao>();
        builder.Services.AddSingleton<TimeAplicacao>();
        builder.Services.AddSingleton<DatabaseAplicacao>();
        builder.Services.AddSingleton<GameAplicacao>();
        builder.Services.AddSingleton<BaseAplicacao>();

        builder.Services.AddTransient<InicioPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<SimularPartidas>();
        builder.Services.AddTransient<AvancarDiasPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
