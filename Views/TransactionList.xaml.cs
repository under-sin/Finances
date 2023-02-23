namespace AppFinances.Views;

public partial class TransactionList : ContentPage
{
	public TransactionList()
	{
		InitializeComponent();
	}

    private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs args)
    {
		// usando o PushAsync a propria api do maui vai gerar o botão de voltar
        Navigation.PushModalAsync(new TransactionAdd());
    }

    void OnButtonClicked_To_TransactionEdit(System.Object sender, System.EventArgs e)
    {
        Navigation.PushModalAsync(new TransactionEdit());
    }
}
