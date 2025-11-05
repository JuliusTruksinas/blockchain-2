using BlockChain.Hashers;

namespace BlockChain.Models;

public class MerkleTree
{
    private readonly IHasher _hasher;

    public MerkleTree(IHasher hasher)
    {
        _hasher = hasher;
    }

    public string GenerateMerkleRoot(List<string> transactionHashes)
    {
        if (transactionHashes.Count == 0)
            throw new ArgumentException("Transaction list cannot be empty", nameof(transactionHashes));

        // If only one hash, it's already the root
        if (transactionHashes.Count == 1)
            return transactionHashes[0];

        List<string> hashes = [.. transactionHashes];

        while (hashes.Count > 1)
        {
            List<string> newHashes = [];

            for (int i = 0; i < hashes.Count; i += 2)
            {
                string left = hashes[i];
                string right = (i + 1 < hashes.Count) ? hashes[i + 1] : left;

                newHashes.Add(_hasher.Hash(left + right));
            }

            hashes = newHashes;
        }

        return hashes[0];
    }
}