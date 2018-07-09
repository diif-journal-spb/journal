using System;
using System.Collections.Generic;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.RFFI
{
  public class RFFIReference
  {
    public static readonly RFFIReferenceComparer Comparer = new RFFIReferenceComparer();
    private readonly IReference myReference;

    public RFFIReference(IReference reference)
    {
      myReference = reference;
    }

    [XmlText]
    public string RefText => myReference.Title.FilterXml();
  }

  public class RFFIReferenceComparer : IEqualityComparer<RFFIReference>
  {
    private string norm(RFFIReference r) => r.RefText.ToLowerInvariant().Replace("\\s+", " ").Trim();

    public bool Equals(RFFIReference x, RFFIReference y) => norm(x) == norm(y);
    public int GetHashCode(RFFIReference obj) => norm(obj).GetHashCode();
  }
}
