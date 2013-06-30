using System;
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

      return sw.ToString()
        .Replace(" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "")
        .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "")
        .Trim()
        ;
    }
  }
}