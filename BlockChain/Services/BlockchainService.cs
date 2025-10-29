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
        var users = _dataGenerator.GenerateUsers(1000);
        var transactions = _dataGenerator.GenerateTransactions(users, 10000);

        int blockCount = 0;
        while (transactions.Any())
        {
            var selectedTransactions = transactions.Take(100).ToList();
            transactions.RemoveRange(0, Math.Min(100, transactions.Count));

            Console.WriteLine($"Mining Block #{++blockCount} with {selectedTransactions.Count} transactions...");
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