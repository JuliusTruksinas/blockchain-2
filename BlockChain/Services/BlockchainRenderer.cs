using System;
using BlockChain.Common;
using BlockChain.Models;

namespace BlockChain.Services;

public class BlockchainRenderer
{
    private readonly Blockchain _blockchain;
    private readonly string _xsltTemplatePath;

    public BlockchainRenderer(Blockchain blockchain, string xsltTemplatePath)
    {
        _blockchain = blockchain;
        _xsltTemplatePath = xsltTemplatePath;
    }

    public Result GenerateHtml()
    {
        var xmlResult = GenerateXmlFromBlockchain();
        if (!xmlResult.IsSuccess)
            return xmlResult.Error;

        var htmlResult = XmlToHtmlConverter.Convert(xmlResult.Value, _xsltTemplatePath);
        if (!htmlResult.IsSuccess)
            return htmlResult.Error;

        var writeResult = WriteToFile(htmlResult.Value);
        if (!writeResult.IsSuccess)
            return writeResult.Error;

        return Result.Success();
    }

    private Result<string> GenerateXmlFromBlockchain()
    {

        var generator = new BlockchainXmlGenerator(_blockchain);
        string xml = generator.Generate();

        if (string.IsNullOrWhiteSpace(xml))
            return ErrorType.XmlGenerationFailed;

        return xml;
    }

    private Result WriteToFile(string html)
    {
        var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Output", "blockchain.html");
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            File.WriteAllText(outputPath, html);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return ErrorType.FileWriteError;
        }
    }
}