using System;
using System.Linq;
using System.Xml;

namespace EugenePetrenko.DataMigration
{
  public class AuthorOrganizationProcessor
  {
    private readonly string myDataDir;
    private readonly ErrorsCount myErrorsCount;

    public AuthorOrganizationProcessor(string dataDir, ErrorsCount errorsCount)
    {
      myDataDir = dataDir;
      myErrorsCount = errorsCount;
    }

    public void Process()
    {
      Util.ProcessFiles(myErrorsCount, myDataDir, "*.authors", file => Util.UpdateXmlDocument(myErrorsCount, file, element =>
      {
        if (element.Name != "authors-xml") throw new Exception(String.Format("Incorrect root element name {0} in {1}", element.Name, file));
        element.SelectNodes("author").Cast<XmlElement>().ForEach(it => ProcessAuthor(file, it));
      }));

      Util.ProcessFiles(myErrorsCount, myDataDir, "*.authors", file => Util.UpdateXmlDocument(myErrorsCount, file, element =>
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
