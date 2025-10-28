using BlockChain.Hashers;

namespace BlockChain.Models;

public class Blockchain
{
    private readonly IHasher _hasher;
    public List<Block> Chain { get; private set; } = new();

    public Blockchain(IHasher hasher)
    {
        _hasher = hasher;
        CreateGenesisBlock();
    }

    private void CreateGenesisBlock()
    {
        var genesisTx = new List<Transaction> { new("GENESIS", "NETWORK", 0, _hasher) };
        var genesisBlock = new Block(genesisTx, "0", 3, _hasher);
        Chain.Add(genesisBlock);
        Console.WriteLine("Genesis block created!");
    }

    public void AddBlock(Block block)
    {
        Chain.Add(block);
        Console.WriteLine($"Block Added: {block.Hash[..10]}...");
    }

    public string GetLatestHash() => Chain.Last().Hash;
}