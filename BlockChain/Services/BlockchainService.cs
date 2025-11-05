using System.Diagnostics;
using BlockChain.Hashers;
using BlockChain.Models;

namespace BlockChain.Services;

public class BlockchainService
{
    private readonly Blockchain _blockchain;
    private readonly IHasher _hasher;
    private readonly DataGenerator _dataGenerator;

    public BlockchainService(IHasher hasher)
    {
        _hasher = hasher;
        _blockchain = new Blockchain(_hasher);
        _dataGenerator = new DataGenerator(_hasher);
    }

    public void Run()
    {
        // Generate users and transactions
        var users = _dataGenerator.GenerateUsers(1000);
        var transactions = _dataGenerator.GenerateTransactions(users, 10000);

        int blockCount = 0;
        while (transactions.Count != 0)
        {
            var selectedTransactions = transactions.Take(100).ToList();
            Console.WriteLine($"Mining Block #{++blockCount} with {selectedTransactions.Count} transactions...");

            // Create candidate blocks
            var candidateBlocks = CreateCandidateBlocks(selectedTransactions, count: 5);

            // Try to mine the blocks with multiple attempts
            Block? minedBlock = MineBlocks(candidateBlocks);

            if (minedBlock is not null)
            {
                _blockchain.AddBlock(minedBlock);
                UpdateBalances(users, selectedTransactions);
                Console.WriteLine($"Block #{blockCount} mined! Hash: {minedBlock.Hash[..10]}...");
                RemoveMinedTransactions(transactions, selectedTransactions);
            }
            else
            {
                Console.WriteLine($"No block mined after 5 attempts. Retrying...");
            }
        }

        Console.WriteLine("All transactions processed!");
        Console.WriteLine($"Total Blocks in Chain: {_blockchain.Chain.Count}");

        RenderBlockchainToHtml();
    }

    private static void RemoveMinedTransactions(List<Transaction> transactions, List<Transaction> selectedTransactions)
    {
        foreach (var transaction in selectedTransactions)
        {
            transactions.Remove(transaction);
        }
    }

    private List<Block> CreateCandidateBlocks(List<Transaction> selectedTransactions, int count, int difficulty = 3)
    {
        var candidateBlocks = new List<Block>();

        for (int i = 0; i < count; i++)
        {
            var candidateBlock = new Block(selectedTransactions, _blockchain.GetLatestHash(), difficulty, _hasher);
            candidateBlocks.Add(candidateBlock);
        }

        return candidateBlocks;
    }

    private Block? MineBlocks(List<Block> candidateBlocks, int maxAttempts = 5)
    {
        Block? minedBlock = null;

        foreach (var candidate in candidateBlocks)
        {
            if (candidate.TryMineBlock(_hasher, maxAttempts))
            {
                minedBlock = candidate;
                break;
            }
        }

        return minedBlock;
    }

    private static void UpdateBalances(List<User> users, List<Transaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            var sender = users.FirstOrDefault(u => u.PublicKey == transaction.Sender);
            var receiver = users.FirstOrDefault(u => u.PublicKey == transaction.Receiver);

            if (sender != null && receiver != null && sender.Balance >= transaction.Amount)
            {
                sender.Balance -= transaction.Amount;
                receiver.Balance += transaction.Amount;
            }
            else
            {
                Console.WriteLine($"Transaction failed: {transaction.Id} | Insufficient funds or invalid user.");
            }
        }
    }
    private void RenderBlockchainToHtml()
    {
        string xsltTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "blockchain.xslt");
        var renderer = new BlockchainRenderer(_blockchain, xsltTemplatePath);
        var renderResult = renderer.RenderHtml();

        if (!renderResult.IsSuccess)
        {
            Console.WriteLine($"Rendering of the blockchain failed: {renderResult.Error}");
            return;
        }

        Console.WriteLine($"Rendered blockchain: {renderResult.Value}");
        OpenFileInBrowser(renderResult.Value);
    }

    private static void OpenFileInBrowser(string filePath)
    {
        try
        {
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening the file: {ex.Message}");
        }
    }
}