using System.Xml;
using System.Xml.Xsl;
using BlockChain.Common;

namespace BlockChain.Services;

public static class XmlToHtmlConverter
{
    public static Result<string> Convert(string xmlContent, string xsltTemplatePath)
    {
        var validationResult = ValidateInput(xmlContent, xsltTemplatePath);
        if (!validationResult.IsSuccess)
            return validationResult.Error;

        var xsltResult = LoadXsltTemplate(xsltTemplatePath);
        if (!xsltResult.IsSuccess)
            return xsltResult.Error;

        return TransformXmlToHtml(xmlContent, xsltResult.Value);
    }

    private static Result ValidateInput(string xmlContent, string xsltTemplatePath)
    {
        if (string.IsNullOrWhiteSpace(xmlContent))
            return ErrorType.InvalidArguments;

        if (!File.Exists(xsltTemplatePath))
            return ErrorType.FileNotFound;

        return Result.Success();
    }

    private static Result<XslCompiledTransform> LoadXsltTemplate(string xsltTemplatePath)
    {
        try
        {
            var xslt = new XslCompiledTransform();
            xslt.Load(xsltTemplatePath);
            return xslt;
        }
        catch (XsltException e)
        {
            return ErrorType.XsltError;
        }
        catch (Exception e)
        {
            return ErrorType.UnexpectedError;
        }
    }

    private static Result<string> TransformXmlToHtml(string xmlContent, XslCompiledTransform xslt)
    {
        try
        {
            using var xmlReader = XmlReader.Create(new StringReader(xmlContent));
            using var output = new StringWriter();

            xslt.Transform(xmlReader, arguments: null, output);

            return output.ToString();
        }
        catch (XsltException e)
        {
            return ErrorType.XsltError;
        }
        catch (Exception e)
        {
            return ErrorType.UnexpectedError;
        }
    }
}