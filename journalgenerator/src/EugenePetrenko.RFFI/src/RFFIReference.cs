using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
  public class RFFIReference
  {
    private readonly IReference myReference;

    public RFFIReference(IReference reference)
    {
      myReference = reference;
    }

    [XmlText]
    public string RefText => myReference.Title.FilterXml();
  }
}
