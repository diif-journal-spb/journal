using System.IO;
using System.Xml.Serialization;

namespace EugenePetrenko.NumberEditor
{
  public static class XmlHelpers
  {
    public static string Serialize<T>(this T t)
    {
      var f = new XmlSerializerFactory();
      
      var sw = new StringWriter();
      f.CreateSerializer(typeof(T)).Serialize(sw, t);

      return sw.ToString();
    }
  }
}