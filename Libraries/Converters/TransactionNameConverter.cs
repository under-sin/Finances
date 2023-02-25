using System;
using System.Globalization;

namespace AppFinances.Libraries.Converters;

public class TransactionNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // retorna a primeira letra da transação para ficar no circulo
        if (value == null)
            return "";

        String TransactionName = (string)value;
        return TransactionName.ToUpper()[0];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

