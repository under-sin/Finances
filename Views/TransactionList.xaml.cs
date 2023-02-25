using AppFinances.Models;
using AppFinances.Repositories;
using CommunityToolkit.Mvvm.Messaging;

namespace AppFinances.Views;

public partial class TransactionList : ContentPage
{
    private ITransactionRepository _repository;

    public TransactionList(
        ITransactionRepository repository)
	{
        _repository = repository;
		InitializeComponent();

        Reload();

        //WeakReferenceMessenger é do Mvvm toolkit. Aqui é uma maneira de comunição sem acoplamento
        //Quando for add um uma transação, o referenceMessenger vai ser chamado e vai atualizar a lista
        //Esse modelo é chamado de published e subscribe. (published é quem enviar a mensagem TransactionAdd)
        WeakReferenceMessenger.Default.Register<string>(this, (e, msg) =>
        {
            Reload();
        });
    }

    private void Reload()
    {
        var items = _repository.GetAll();

        // populando a lista do collenction
        CollectionViewTransactions.ItemsSource = items;

        double income = items.Where(x => x.Type == Models.TransactionType.Income).Sum(x => x.Value);
        double expenses = items.Where(x => x.Type == Models.TransactionType.Expenses).Sum(x => x.Value);
        double balance = income - expenses;

        LabelIncome.Text = income.ToString("C");
        LabelExpenses.Text = expenses.ToString("C");
        LabelBalance.Text = balance.ToString("C");  
    }

    private void OnButtonClicked_To_TransactionAdd(object sender, EventArgs args)
    {
        // outra forma de pegar a instancia da view
        // dessa forma sempre que abrirmos a tela de add vamos ter uma nova instância
        // dela com os campos limpos
        var transactionAdd = Handler.MauiContext.Services.GetService<TransactionAdd>();

        // usando o PushAsync a propria api do maui vai gerar o botão de voltar
        Navigation.PushModalAsync(transactionAdd);
    } 

    void TapGestureRecognizerTapped_To_TransactionEdit(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        //pegando o objeto que foi passado no CommandParameter com o sender
        //fazendo os devidos casting até pegando o commandparamter que é um objeto do tipo transaction
        var grid = (Grid)sender;
        var gesture = (TapGestureRecognizer)grid.GestureRecognizers[0];
        Transaction transaction = (Transaction)gesture.CommandParameter;

        var transactionEdit = Handler.MauiContext.Services.GetService<TransactionEdit>();
        transactionEdit.SetTransactionToEdit(transaction);
        Navigation.PushModalAsync(transactionEdit);
    }

    private async void TapGestureRecognizerTapped_To_Delete(System.Object sender, Microsoft.Maui.Controls.TappedEventArgs e)
    {
        await AnimationBorder((Border)sender, true);

        // mostra uma alert na tela, caso o usuário escolha sim "true" deleta o item selecionado
        var result = await App.Current.MainPage.DisplayAlert("Excluir", "Tem certeza que seja excluir?", "Sim", "Não");

        if (result)
        {
            //e.parameter é outra forma de pegar o CommandParameter que está passando o objeto
            Transaction transaction = (Transaction)e.Parameter;
            _repository.Delete(transaction);

            Reload();
        }
        else
        {
            await AnimationBorder((Border)sender, false);
        }
    }

    private Color _borderOriginalBackgroundColor;
    private String _textOriginalValue;
    public async Task AnimationBorder(Border border, bool IsDeleteAnimation)
    {
        // quando o elemente só tem 1 filho, usamos o content,
        // quando tempo mais de um, usamos o Child
        var label = (Label)border.Content;

        if (IsDeleteAnimation)
        {
            _borderOriginalBackgroundColor = border.BackgroundColor;
            _textOriginalValue = label.Text;

            await border.RotateYTo(90, 300);

            border.BackgroundColor = Colors.Red;

            label.TextColor = Colors.White;
            label.Text = "X";

            await border.RotateYTo(180, 300);
        }
        else
        {
            await border.RotateYTo(90, 300);

            border.BackgroundColor = _borderOriginalBackgroundColor;

            label.TextColor = Colors.Black;
            label.Text = _textOriginalValue;

            await border.RotateYTo(0, 300);
        }
    }
}
