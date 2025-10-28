namespace BlockChain.Models;

public class User
{
    public string Name { get; set; }
    public string PublicKey { get; set; }
    public decimal Balance { get; set; }

    public User(string name, string publicKey, decimal balance)
    {
        Name = name;
        PublicKey = publicKey;
        Balance = balance;
    }

    public override string ToString()
    {
        return $"{Name} ({PublicKey[..6]}...) - Balance: {Balance}";
    }
}