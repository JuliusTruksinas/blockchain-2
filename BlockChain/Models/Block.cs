using BlockChain.Hashers;

namespace BlockChain.Models;

public class Block
{
    public BlockHeader Header { get; private set; }
    public List<Transaction> Transactions { get; private set; }
    public string Hash { get; private set; }

    public Block(
        List<Transaction> transactions,
        string previousHash,
        int difficulty,
        IHasher hasher)
    {
        Transactions = transactions;

        var transactionHashes = transactions.Select(t => t.Id).ToList();
        var merkleTree = new MerkleTree(hasher);
        string transactionsMerkleRoot = merkleTree.GenerateMerkleRoot(transactionHashes);

        Header = new BlockHeader(previousHash, transactionsMerkleRoot, difficulty);
        Hash = MineBlock(hasher);
    }

    private string MineBlock(IHasher hasher)
    {
        string hash;
        string prefix = new string('0', Header.Difficulty);
        int nonce = 0;

        do
        {
            Header.Nonce = nonce++;
            string input = $"{Header.PreviousHash}{Header.Timestamp}{Header.Version}{Header.TransactionsHash}{Header.Nonce}";
            hash = hasher.Hash(input);
        } while (!hash.StartsWith(prefix));

        return hash;
    }

    public override string ToString()
    {
        return $"Block hash: {Hash[..10]}..., Prev: {Header.PreviousHash[..10]}..., Transaction count: {Transactions.Count}, Nonce: {Header.Nonce}";
    }
}
