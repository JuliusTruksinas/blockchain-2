using BlockChain.Hashers;
using BlockChain.Models;

namespace BlockChain.Services;

public class DataGenerator
{
    private readonly IHasher _hasher;
    private readonly Random _random = new();

    public DataGenerator(IHasher hasher)
    {
        _hasher = hasher;
    }

    public List<User> GenerateUsers(int count)
    {
        var users = new List<User>(count);

        for (int i = 0; i < count; i++)
        {
            string name = $"User{i + 1}";
            string pubKey = _hasher.Hash(name + Guid.NewGuid());
            decimal balance = _random.Next(100, 1_000_000);
            users.Add(new User(name, pubKey, balance));
        }

        return users;
    }

    public List<Transaction> GenerateTransactions(List<User> users, int count)
    {
        var transactions = new List<Transaction>(count);

        for (int i = 0; i < count; i++)
        {
            var sender = users[_random.Next(users.Count)];
            var receiver = users[_random.Next(users.Count)];

            while (receiver.PublicKey == sender.PublicKey)
                receiver = users[_random.Next(users.Count)];

            decimal amount = _random.Next(1, (int)sender.Balance / 2 + 1);

            transactions.Add(new Transaction(sender.PublicKey, receiver.PublicKey, amount, _hasher));
        }

        return transactions;
    }
}