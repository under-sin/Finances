using System;
using System.Globalization;
using AppFinances.Models;

namespace AppFinances.Libraries.Converters;

public class TransactionValueColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // altera a cor do valor da transação de acordo com o tipo
        Transaction transaction = (Transaction)value;
        if (transaction == null)
            return Colors.Black;
        else if (transaction.Type == TransactionType.Income)
            return Color.FromArgb("#FF939E5A");
        else
            return Color.FromArgb("#FFFF5232");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

