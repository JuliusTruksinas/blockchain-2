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
            var selectedTransactions = GetNextTransactions(ref transactions);
            Console.WriteLine($"Mining Block #{++blockCount} with {selectedTransactions.Count} transactions...");

            // Create and mine candidate blocks
            var minedBlock = MineBlock(selectedTransactions);

            transactions.RemoveRange(0, Math.Min(100, transactions.Count));

            var block = new Block(selectedTransactions, _blockchain.GetLatestHash(), 3, _hasher);
            _blockchain.AddBlock(block);

            UpdateBalances(users, selectedTransactions);
            Console.WriteLine($"Block #{blockCount} mined! Hash: {block.Hash[..10]}...");
        }

        Console.WriteLine("All transactions processed!");
        Console.WriteLine($"Total Blocks in Chain: {_blockchain.Chain.Count}");

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

    private List<Transaction> GetNextTransactions(ref List<Transaction> transactions)
    {
        var selectedTransactions = transactions.Take(100).ToList();
        transactions.RemoveRange(0, Math.Min(100, transactions.Count));
        return selectedTransactions;
    }

    private List<Block> CreateCandidateBlocks(List<Transaction> selectedTransactions)
    {
        var candidateBlocks = new List<Block>();

        for (int i = 0; i < 5; i++)  // Try 5 candidate blocks
        {
            var candidateBlock = new Block(selectedTransactions, _blockchain.GetLatestHash(), 3, _hasher);
            candidateBlocks.Add(candidateBlock);
        }

        return candidateBlocks;
    }

    private Block? MineBlock(List<Transaction> selectedTransactions)
    {
        var candidateBlocks = CreateCandidateBlocks(selectedTransactions);

        foreach (var candidate in candidateBlocks)
        {
            if (candidate.Hash.StartsWith(new string('0', 3)))
            {
                return candidate;
            }
        }

        return null;
    }

    private void UpdateBalances(List<User> users, List<Transaction> transactions)
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

    private void OpenFileInBrowser(string filePath)
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