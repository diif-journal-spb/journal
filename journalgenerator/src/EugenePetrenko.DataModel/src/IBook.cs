using System.Xml;

namespace EugenePetrenko.DataModel
{
  public interface IBook : ILocalizedEntityWithDate<ILocalizedBook>
  {
    string Image { get; }
  }

  internal class BookImpl : LocalizedEntitySequenceWithDate<ILocalizedBook>, IBook
  {
    private string myImage;

    public BookImpl(XmlNode el, IXmlDataLoader loader) : base(el, loader)
    {      
    }

    protected override void Constructor(XmlNode node, IXmlDataLoader loader)
    {
      base.Constructor(node, loader);
      myImage = node.SelectSingleNode("image/text()").Value.Trim();
    }

    protected override ILocalizedBook CreateElement(XmlNode node, IXmlDataLoader loader)
    {
      return new LocalizedBookImpl(Date, myImage, node, loader);
    }

    public string Image
    {
      get { return myImage; }
    }
  }
}