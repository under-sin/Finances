using AppFinances.Views;

namespace AppFinances;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		// adicionando navegação no aplicativo
		MainPage = new NavigationPage(new TransactionList());
	}
}

