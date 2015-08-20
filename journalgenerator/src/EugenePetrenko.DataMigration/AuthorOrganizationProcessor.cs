using System;
using System.Linq;
using System.Xml;

namespace EugenePetrenko.DataMigration
{
  public class AuthorOrganizationProcessor
  {
    private readonly string myDataDir;

    public AuthorOrganizationProcessor(string dataDir)
    {
      myDataDir = dataDir;
    }

    public void Process()
    {
      Util.ProcessFiles(myDataDir, "*.authors", file => Util.UpdateXmlDocument(file, element =>
      {
        if (element.Name != "authors-xml") throw new Exception(String.Format("Incorrect root element name {0} in {1}", element.Name, file));
        element.SelectNodes("author").Cast<XmlElement>().ForEach(it => ProcessAuthor(file, it));
      }));

      Util.ProcessFiles(myDataDir, "*.authors", file => Util.UpdateXmlDocument(file, element =>
      {
        if (element.Name != "authors-xml") throw new Exception(String.Format("Incorrect root element name {0} in {1}", element.Name, file));
        element.SelectNodes("author").Cast<XmlElement>().ForEach(it => ProcessAuthor(file, it));
      }));
    }


    private void ProcessAuthor(string reference, XmlElement author)
    {
        
    }
  }
}