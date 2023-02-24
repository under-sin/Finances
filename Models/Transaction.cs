using LiteDB;

namespace AppFinances.Models;

public class Transaction
{
    [BsonId] // define o id usando o litedb
    public int Id { get; set; }
    public TransactionType Type { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Value { get; set; }
}

public enum TransactionType
{
    Income,
    Expenses
}
