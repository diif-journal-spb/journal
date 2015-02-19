using System;
using System.Collections.Generic;
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

      var schema = assembly.GetManifestResourceStream("EugenePetrenko.RFFI.src.schemas.JournalArticulus.xsd");
      if (schema == null) throw new Exception("Failed to load schema from resources");
     
      var documentCopy = (XmlDocument) document.CloneNode(true);
       using (schema)
      {
        documentCopy.Schemas.Add("", XmlReader.Create(schema));        
      }
      var errors = new List<String>();

      documentCopy.Validate((sender, args) =>
      {
        if (args.Severity != XmlSeverityType.Error) return;

        errors.Add(args.Message + "  " + args.Exception);
        Console.Out.WriteLine(args.Message + "  " + args.Exception);
      });

      if (errors.Any())
      {
        throw new Exception("Invalid XML: " + errors);
      }
    }
  }
}