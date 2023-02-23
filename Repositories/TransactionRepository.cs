using AppFinances.Models;
using LiteDB;

namespace AppFinances.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly LiteDatabase _database;
    private readonly string collectionName = "transactions";

    // trabalhando com injeção de dependência
    public TransactionRepository(LiteDatabase database)
    {
        _database = database;
    }

    public List<Transaction> GetAll()
    {
        return _database
            .GetCollection<Transaction>(collectionName)
            .Query()
            .OrderByDescending(x => x.Date)
            .ToList();
    }

    public void Add(Transaction transaction)
    {
        var col = _database.GetCollection<Transaction>(collectionName);
        col.Insert(transaction);
        col.EnsureIndex(x => x.Date);
    }

    public void Update(Transaction transaction)
    {
        var col = _database.GetCollection<Transaction>(collectionName);
        col.Update(transaction);
    }

    public void Delete(Transaction transaction)
    {
        var col = _database.GetCollection<Transaction>(collectionName);
        col.Delete(transaction.Id);
    }
}
