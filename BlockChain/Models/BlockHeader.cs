namespace BlockChain.Models;

public class BlockHeader
{
    public string PreviousHash { get; set; }
    public DateTime Timestamp { get; set; }
    public string Version { get; set; } = "v0.1";
    public string TransactionsHash { get; set; }
    public int Nonce { get; set; }
    public int Difficulty { get; set; }

    public BlockHeader(string previousHash, string transactionsHash, int difficulty)
    {
        PreviousHash = previousHash;
        TransactionsHash = transactionsHash;
        Timestamp = DateTime.UtcNow;
        Difficulty = difficulty;
    }
}