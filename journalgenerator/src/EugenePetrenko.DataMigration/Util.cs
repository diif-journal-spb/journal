using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace EugenePetrenko.DataMigration
{
  public static class Util {
    public static void ForEach<T>(this IEnumerable<T> data, Action<T> it)
    {
      foreach (var t in data)
      {
        it(t);
      }
    }

    public static void ForEach<T>(this IEnumerable<T> data, ErrorsCount ec, Action<T> it)
    {
      foreach (var _t in data)
      {
        var t = _t;
        ec.Filter("" + t, () => it(t));
      }
    }

    public static void ProcessFiles(ErrorsCount ec, string baseDir, string pattern, Action<string> processor)
    {
      foreach (var _file in Directory.GetFiles(baseDir, pattern))
      {
        var file = _file;
        Console.Out.WriteLine("Processing " + file);
        ec.Catch(file, 
          () => processor(file), 
          e => e.Log("Failed to process {0}.\n{1}\n{2}", file, e.Message, e.Exception));
      }
    }

    public static void UpdateOrCreateXmlDocument(ErrorsCount ec, string file, Action<XmlElement> update, Action<XmlDocument> init)
    {
      if (!File.Exists(file))
      {
        var doc = new XmlDocument();
        init(doc);
        SaveDocument(ec, file, doc);
      }

      UpdateXmlDocument(ec, file, update);
    }

    public static void UpdateXmlDocument(ErrorsCount ec, string file, Action<XmlElement> update)
    {
      var doc = new XmlDocument();
      doc.PreserveWhitespace = true;

      ec.Catch("Load " + file, 
        () => doc.Load(file), 
        e => e.LogAndThrow("Failed to load XML document form " + file));

      update(doc.DocumentElement);

      SaveDocument(ec, file, doc);
    }

    private static void SaveDocument(ErrorsCount ec, string file, XmlDocument doc)
    {
      var ms = new MemoryStream();
      ec.Catch("Generate " + file, () =>
      {
        using (var w = new StreamWriter(ms, new UTF8Encoding(false)))
        {
          doc.Save(w);
        }
      }, e => e.LogAndThrow("Failed to generate XML for " + file));

      ec.Catch("Save " + file, () =>
      {
        using (var w = File.Create(file))
        {
          var text = Encoding.UTF8.GetString(ms.ToArray()).Trim() + "\r\n";
          var buff = Encoding.UTF8.GetBytes(text);
          w.Write(buff, 0, buff.Length);
        }
      },
      e => e.LogAndThrow( "Failed to save XML for " + file));
    }
  }
}
