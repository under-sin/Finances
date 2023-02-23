using AppFinances.Repositories;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace AppFinances;

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
			})
			.RegisterDatabaseAndRepository();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	public static MauiAppBuilder RegisterDatabaseAndRepository(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddSingleton<LiteDatabase>(
			options => {
				return new LiteDatabase($"Filename={AppSettings.DatabasePath};Connection=Shared");
			}
		);
		mauiAppBuilder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
		return mauiAppBuilder;
	}
}

