using System;
using System.Globalization;
using AppFinances.Models;

namespace AppFinances.Libraries.Converters;

public class TrasactionValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // adiciona o sinal de menos (-) quando a transação é expense
        Transaction transaction = (Transaction)value;
        if (transaction == null)
            return "";
        else if (transaction.Type == TransactionType.Income)
            return transaction.Value.ToString("C");
        else
            return $"- {transaction.Value.ToString("C")}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

