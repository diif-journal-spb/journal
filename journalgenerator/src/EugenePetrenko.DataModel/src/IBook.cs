using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IBook : ILocalizedEntityWithDate<ILocalizedBook>
  {
    string Image { get; }
    string Pdf { get; }
  }

  internal class BookImpl : LocalizedEntitySequenceWithDate<ILocalizedBook>, IBook
  {
    public BookImpl(XmlNode el, IXmlDataLoader loader) : base(el, loader)
    {      
    }

    protected override void Constructor(XmlNode node, IXmlDataLoader loader)
    {
      base.Constructor(node, loader);
      Image = node.ReadText("image/text()");
      Pdf = node.ReadText("pdf/text()");
    }

    protected override ILocalizedBook CreateElement(XmlNode node, IXmlDataLoader loader)
    {
      return new LocalizedBookImpl(Date, this, node, loader);
    }

    public string Image { get; private set;}

    public string Pdf { get; private set;}
  }
}