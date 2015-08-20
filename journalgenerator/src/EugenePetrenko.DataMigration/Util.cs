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

    public static void ProcessFiles(string baseDir, string pattern, Action<string> processor)
    {
      foreach (var file in Directory.GetFiles(baseDir, pattern))
      {
        Console.Out.WriteLine("Processing " + file);
        try
        {
          processor(file);
        }
        catch (Exception e)
        {
          Console.Out.WriteLine("Failed to process {0}.\n{1}\n{2}", file, e.Message, e);
        }
      }
    }

    public static void UpdateOrCreateXmlDocument(string file, Action<XmlElement> update, Action<XmlDocument> init)
    {
      if (!File.Exists(file))
      {
        var doc = new XmlDocument();
        init(doc);
        SaveDocument(file, doc);
      }

      UpdateXmlDocument(file, update);
    }

    public static void UpdateXmlDocument(string file, Action<XmlElement> update)
    {
      var doc = new XmlDocument();
      doc.PreserveWhitespace = true;

      doc.Load(file);

      update(doc.DocumentElement);

      SaveDocument(file, doc);
    }

    private static void SaveDocument(string file, XmlDocument doc)
    {
      var ms = new MemoryStream();
      using (var w = new StreamWriter(ms, new UTF8Encoding(false)))
      {
        doc.Save(w);       
      }

      using (var w = File.Create(file))
      {
        var text = Encoding.UTF8.GetString(ms.ToArray()).Trim() + "\r\n";
        var buff = Encoding.UTF8.GetBytes(text);
        w.Write(buff, 0, buff.Length);
      }
    }
  }
}
