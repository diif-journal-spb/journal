using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using JetBrains.Annotations;

namespace EugenePetrenko.RFFI
{
  public static class RFFISchemaValidator
  {
    public static void EnsureRFFIValid([NotNull] XmlDocument document)
    {
      var assembly = typeof (RFFISchemaValidator).Assembly;


      var settings = new XmlReaderSettings();
      settings.ValidationType = ValidationType.Schema;

      using (var schema = assembly.GetManifestResourceStream("EugenePetrenko.RFFI.src.schemas.JournalArticulus.xsd"))
      {
        if (schema == null) throw new Exception("Failed to load schema from resources");
        settings.Schemas.Add(null, XmlReader.Create(schema));
      }


      var errors = new List<String>();
      settings.ValidationEventHandler += (sender, args) =>
      {
        if (args.Severity != XmlSeverityType.Error) return;

        errors.Add(args.Message + "  " + args.Exception);
        Console.Out.WriteLine(args.Message + "  " + args.Exception);
      };

      var sw = new StringWriter();
      document.Save(sw);
      sw.Close();

      using (var reader = XmlReader.Create(new StringReader(sw.ToString()), settings))
      {
        while (reader.Read()) ;
      }

      if (errors.Any())
      {
        throw new Exception("Invalid XML: " + String.Join("\r\n", errors.ToArray()));
      }

      Action<XmlDocument>[] validators =
      {
        ValidateNo2072909X,
        ValidateCodeNEB,
        ValidateISSN,
        ValidatetTitleId,
      };

      foreach (var validator in validators)
      {
        var doc = new XmlDocument();
        doc.LoadXml(sw.ToString());
        validator(doc);
      }
    }


    private static void ValidateCodeNEB(XmlDocument doc)
    {
      var neb = doc.SelectSingleNode("journal/codeNEB/text()");
      if (neb == null)
      {
        throw new Exception("Document does not contain codeNEB");
      }

      if (neb.Value != "18172172")
      {
        throw new Exception("Document contains invalid codeNEB: " + neb.Value);
      }
    }

    private static void ValidateISSN(XmlDocument doc)
    {
      var neb = doc.SelectSingleNode("journal/issn/text()");
      if (neb == null)
      {
        throw new Exception("Document does not contain issn");
      }

      if (neb.Value != "1817-2172")
      {
        throw new Exception("Document contains invalid issn: " + neb.Value);
      }
    }

    private static void ValidatetTitleId(XmlDocument doc)
    {
      var neb = doc.SelectSingleNode("journal/titleid/text()");
      if (neb == null)
      {
        throw new Exception("Document does not contain issn");
      }

      if (neb.Value != "7282")
      {
        throw new Exception("Document contains invalid issn: " + neb.Value);
      }
    }

    private static void ValidateNo2072909X(XmlDocument doc)
    {
      if (doc.OuterXml.Contains("2072909X"))
      {
        throw new Exception("Document must not contain onlder ID: '2072909X'");
      }
    }
  }
}