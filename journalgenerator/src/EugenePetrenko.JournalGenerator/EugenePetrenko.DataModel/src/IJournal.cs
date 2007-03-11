using System.Collections.Generic;
using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IJournal : IEntity
  {
    INumber[] Numbers { get; }  
  }

  public class Journal : Entity, IJournal
  {
    private INumber[] myNumbers;

    public Journal(XmlElement element, XmlDataLoader loader) : base(loader.EntityGenerator)
    {
      List<INumber> numbers = new List<INumber>();
      foreach (XmlElement node in element.SelectNodes("number"))
      {
        numbers.Add(loader.ParseNumber(node));
      }
      myNumbers = numbers.ToArray();
    }

    public INumber[] Numbers
    {
      get { return myNumbers; }
    }
  }
}