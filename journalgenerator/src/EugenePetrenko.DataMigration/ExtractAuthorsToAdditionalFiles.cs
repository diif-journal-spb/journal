using System;
using System.Linq;
using System.Xml;

namespace EugenePetrenko.DataMigration
{
  public static class ExtractAuthorsToAdditionalFiles
  {
    public static void Process(ErrorsCount ec, string dataDir)
    {
      Util.ProcessFiles(ec, dataDir, "*.number", file => Util.UpdateXmlDocument(ec, file, root =>
      {
        if (root.Name != "number") throw new Exception(String.Format("Incorrect root element name {0} in {1}", root.Name, file));

        foreach (XmlElement authors in root.SelectNodes("//authors-xml"))
        {
          foreach (XmlElement author in authors.SelectNodes("author"))
          {
            Console.Out.WriteLine("Authors declared in {0}", file);

            var authorsFile = file.Replace(".number", ".authors");

            Util.UpdateOrCreateXmlDocument(ec, authorsFile,
              authros =>
              {
                var copy = authros.OwnerDocument.ImportNode(author, true);
                authros.AppendChild(authros.OwnerDocument.CreateSignificantWhitespace("\r\n"));
                authros.AppendChild(copy);
              },
              doc => doc.AppendChild(doc.CreateElement("authors-xml")));

            author.RemoveSelf();
          }

          if (authors.SelectNodes("*").Count == 0)
          {
            authors.RemoveSelf();
          }
        }
      }));
    }
  }
}
