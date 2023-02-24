using System.Text;
using AppFinances.Models;
using AppFinances.Repositories;
using CommunityToolkit.Mvvm.Messaging;

namespace AppFinances.Views;

public partial class TransactionEdit : ContentPage
{
    private Transaction _transaction;
    private readonly ITransactionRepository _repository;

    public TransactionEdit(ITransactionRepository repository)
	{
		InitializeComponent();
        _repository = repository;
    }

    public void SetTransactionToEdit(Transaction transaction)
    {
        //Aqui vamos preencher o formulário com os dados que vieram do TapGestureRecognizerTapped_To_TransactionEdit
        _transaction = transaction;

        if (transaction.Type == TransactionType.Income)
            RadioIncome.IsChecked = true;
        else
            RadioExpense.IsChecked = true;

        EntryName.Text = transaction.Name;
        EntryValue.Text = transaction.Value.ToString("n2");
        DatePickerDate.Date = transaction.Date.Date;
    }

    void TapGestureRecognizerTapped_ToClose(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void OnClickedButton_Save(System.Object sender, System.EventArgs e)
    {
        if (IsValidDate() == false)
            return;

        UpdateTransactionInDatabase();

        Navigation.PopModalAsync();

        // Published - vai enviar a mensagem para o transactionList e atualizar a lista com o novo valor
        WeakReferenceMessenger.Default.Send<string>(string.Empty);
    }

    private void UpdateTransactionInDatabase()
    {
        Transaction transaction = new Transaction()
        {
            Id = _transaction.Id,
            Name = EntryName.Text,
            Value = double.Parse(EntryValue.Text),
            Date = DatePickerDate.Date,
            Type = RadioIncome.IsChecked ? TransactionType.Income : TransactionType.Expenses,
        };

        _repository.Update(transaction);
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
