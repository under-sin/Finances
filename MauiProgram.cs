using AppFinances.Repositories;
using AppFinances.Views;
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
			.RegisterDatabaseAndRepository()
			.RegisterViews();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	// injeção de dependência
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

	// injeção de dependência das views, assim tempos acesso ao repository
    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<TransactionAdd>();
        mauiAppBuilder.Services.AddTransient<TransactionEdit>();
        mauiAppBuilder.Services.AddTransient<TransactionList>();
        return mauiAppBuilder;
    }
}

