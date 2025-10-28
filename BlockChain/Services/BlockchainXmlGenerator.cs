using System.Xml.Linq;
using BlockChain.Models;

namespace BlockChain.Services;

public class BlockchainXmlGenerator
{
    private readonly Blockchain _blockchain;

    public BlockchainXmlGenerator(Blockchain blockchain)
    {
        _blockchain = blockchain;
    }

    public string Generate()
    {
        var xml = GenerateBlockchainElement();
        return xml.ToString();
    }

    private XElement GenerateBlockchainElement()
    {
        var blockchainEl = new XElement("Blockchain");

        for (int i = 0; i < _blockchain.Chain.Count; i++)
        {
            blockchainEl.Add(
                new XElement("Block",
                    new XAttribute("Index", i),
                    GenerateHeaderElement(_blockchain.Chain[i]),
                    new XElement("Hash", _blockchain.Chain[i].Hash),
                    GenerateTransactionsElement(_blockchain.Chain[i]))
            );
        }

        return blockchainEl;
    }

    private static XElement GenerateHeaderElement(Block block)
    {
        return new XElement("Header",
            new XElement("PreviousHash", block.Header.PreviousHash),
            new XElement("Timestamp", block.Header.Timestamp.ToString("o")), // ISO 8601
            new XElement("Version", block.Header.Version),
            new XElement("TransactionsHash", block.Header.TransactionsHash),
            new XElement("Nonce", block.Header.Nonce),
            new XElement("Difficulty", block.Header.Difficulty)
        );
    }

    private static XElement GenerateTransactionsElement(Block block)
    {
        return new XElement("Transactions",
            block.Transactions.Select(GenerateTransactionElement)
        );
    }

    private static XElement GenerateTransactionElement(Transaction transaction)
    {
        return new XElement("Transaction",
            new XElement("Id", transaction.Id),
            new XElement("Sender", transaction.Sender),
            new XElement("Receiver", transaction.Receiver),
            new XElement("Amount", transaction.Amount)
        );
    }
}