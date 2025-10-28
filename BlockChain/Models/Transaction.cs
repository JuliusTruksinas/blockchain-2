using BlockChain.Hashers;

namespace BlockChain.Models;

public class Transaction
{
    public string Id { get; private set; }
    public string Sender { get; set; }
    public string Receiver { get; set; }
    public decimal Amount { get; set; }

    public Transaction(string sender, string receiver, decimal amount, IHasher hasher)
    {
        Sender = sender;
        Receiver = receiver;
        Amount = amount;
        Id = hasher.Hash($"{sender}{receiver}{amount}{Guid.NewGuid()}");
    }

    public override string ToString()
    {
        return $"{Id[..8]}... | {Sender[..6]} -> {Receiver[..6]} | {Amount}";
    }
}