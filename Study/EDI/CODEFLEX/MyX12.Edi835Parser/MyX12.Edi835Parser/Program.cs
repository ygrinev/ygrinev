using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Xsl;
using OopFactory.X12.Parsing;
using OopFactory.X12.Parsing.Model;

namespace MyX12.Edi835Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Stream transformStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MyX12.Edi835Parser.X12-835-To-CSV.xslt");
            Stream inputStream = new FileStream(args[0], FileMode.Open, FileAccess.Read);
            Stream outputFile = new FileStream(args[1], FileMode.Create, FileAccess.Write);

            X12Parser parser = new X12Parser();
            Interchange interchange = parser.Parse(inputStream);
            string xml = interchange.Serialize();

            var transform = new XslCompiledTransform();
            transform.Load(XmlReader.Create(transformStream));

            transform.Transform(XmlReader.Create(new StringReader(xml)), new XsltArgumentList(), outputFile);
        }
    }
}
