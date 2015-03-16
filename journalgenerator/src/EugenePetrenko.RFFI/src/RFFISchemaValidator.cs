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
    }
  }
}