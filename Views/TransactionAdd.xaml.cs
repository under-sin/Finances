using System.Text;
using AppFinances.Models;
using AppFinances.Repositories;
using CommunityToolkit.Mvvm.Messaging;

namespace AppFinances.Views;

public partial class TransactionAdd : ContentPage
{
    private readonly ITransactionRepository _repository;

	public TransactionAdd(ITransactionRepository repository)
	{
        _repository = repository;
		InitializeComponent();
	}

    void TapGestureRecognizerTapped_ToClose(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
		// feixa a modal ao "tap" no X
		Navigation.PopModalAsync();
    }

    private void OnClickedButton_Save(System.Object sender, System.EventArgs e)
    {
        if (IsValidDate() == false)
            return;

        SaveTransactionInDatabase();

        Navigation.PopModalAsync();

        // Published - vai enviar a mensagem para o transactionList e atualizar a lista com o novo valor
        WeakReferenceMessenger.Default.Send<string>(string.Empty);

        var count = _repository.GetAll().Count;
        App.Current.MainPage.DisplayAlert("Alert", $"Exitem {count} registro(s) no banco ", "OK");
    }

    private void SaveTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Name = EntryName.Text,
            Value = double.Parse(EntryValue.Text),
            Date = DatePickerDate.Date,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
        }; 

        _repository.Add(transaction);
    }

    private bool IsValidDate()
    {
        // o x:Name define uma variável, assim conseguimos acessar os valores do formulário

        bool valid = true;
        double result;
        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrEmpty(EntryName.Text) || string.IsNullOrWhiteSpace(EntryName.Text))
        {
            sb.AppendLine("O campo Nome é obrigatório");
            valid = false;
        }
        if (string.IsNullOrEmpty(EntryValue.Text) || string.IsNullOrWhiteSpace(EntryValue.Text))
        {
            sb.AppendLine("O campo Valor é obrigatório");
            valid = false;
        }
        if (!string.IsNullOrEmpty(EntryValue.Text) && !double.TryParse(EntryValue.Text, out result))
        {
            sb.AppendLine("O campo Valor é inválido");
            valid = false;
        }

        if (valid == false)
        {
            LabelError.IsVisible = true;
            LabelError.Text = sb.ToString();
        }

        return valid;
    }
}
