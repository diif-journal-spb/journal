using System;
using System.Linq;
using System.Xml;

namespace EugenePetrenko.DataMigration
{
  internal class ExtractAuthorsToAdditionalFiles
  {
    private readonly string myDataDir;
    private readonly ErrorsCount myErrorsCount;

    public ExtractAuthorsToAdditionalFiles(string dataDir, ErrorsCount errorsCount)
    {
      myDataDir = dataDir;
      myErrorsCount = errorsCount;
    }

    public void Process()
    {
      Util.ProcessFiles(myErrorsCount, myDataDir, "*.number", file => Util.UpdateXmlDocument(myErrorsCount, file, element =>
      {
        if (element.Name != "number") throw new Exception(String.Format("Incorrect root element name {0} in {1}", element.Name, file));
        element.SelectNodes("//authors-xml/author").Cast<XmlElement>().ForEach(it => ProcessAuthor(file, it));

        if (element.SelectNodes("*").Count == 0)
        {
          element.ParentNode.RemoveChild(element);
        }
      }));
    }

    private void ProcessAuthor(string file, XmlElement author)
    {
      Console.Out.WriteLine("Authors declared in {0}", file);

      var authorsFile = file.Replace(".number", ".authors");
      Util.UpdateOrCreateXmlDocument(myErrorsCount, authorsFile,
        authros =>
        {
          var copy = authros.OwnerDocument.ImportNode(author, true);
          authros.AppendChild(copy);
        },
        doc => doc.AppendChild(doc.CreateElement("authors-xml")));

      author.ParentNode.RemoveChild(author);
    }
  }
}
