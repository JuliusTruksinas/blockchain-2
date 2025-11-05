using BlockChain.Hashers;

namespace BlockChain.Models;

public class Block
{
    public BlockHeader Header { get; private set; }
    public List<Transaction> Transactions { get; private set; }
    public string Hash { get; private set; } // Will be set when mined

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
        Hash = string.Empty;
    }

    public bool TryMineBlock(IHasher hasher, int maxAttempts)
    {
        string prefix = new string('0', Header.Difficulty);
        int nonce = 0;
        string hash;

        // Perform the mining process (Proof of Work)
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            Header.Nonce = nonce++;
            string input = $"{Header.PreviousHash}{Header.Timestamp}{Header.Version}{Header.TransactionsHash}{Header.Nonce}";
            hash = hasher.Hash(input);

            if (hash.StartsWith(prefix))
            {
                // If a valid hash is found, set it and return true
                Hash = hash;
                return true;
            }
        }

        // If no valid hash is found within the maxAttempts, return false
        return false;
    }

    public override string ToString()
    {
        return $"Block hash: {Hash[..10]}..., Prev: {Header.PreviousHash[..10]}..., Transaction count: {Transactions.Count}, Nonce: {Header.Nonce}";
    }
}
