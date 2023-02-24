using AppFinances.Views;

namespace AppFinances;

public partial class App : Application
{
	public App(TransactionList listPage)
	{
		InitializeComponent();
		// adicionando navegação no aplicativo
		MainPage = new NavigationPage(listPage);
	}
}

