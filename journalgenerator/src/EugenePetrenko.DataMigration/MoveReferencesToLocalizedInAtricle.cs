using System;
using System.Linq;
using System.Xml;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.DataMigration
{
  public static class MoveReferencesToLocalizedInAtricle
  {
    public static void Process(ErrorsCount ec, string dataDir)
    {
      Console.Out.WriteLine("===============");
      Console.Out.WriteLine("Moving references to article...");

      Util.ProcessFiles(ec, dataDir, "*.number", file => Util.UpdateXmlDocument(ec, file, root =>
      {
        if (root.Name != "number") throw new Exception(String.Format("Incorrect root element name {0} in {1}", root.Name, file));
        foreach (var article in root.SelectNodes("article").Elements())
        {
          var references = article.SelectNodes("references").Elements().ToList();
          if (references.IsEmpty()) continue;

          var articleInfo = article.SelectNodes("articleInfo[@lang='EN']").Elements().FirstOrDefault();
          if (articleInfo == null)
          {
            ec.Error("Article does not have EN localization: {0}", file);
            continue;
          }
          
          foreach (XmlElement refElement in references)
          {
            articleInfo.AppendChild(articleInfo.OwnerDocument.ImportNode(refElement, true));
            refElement.RemoveSelf();
          }

          Console.Out.WriteLine("References updated in {0}", file);         
        }
      }));
    }
  }
}
