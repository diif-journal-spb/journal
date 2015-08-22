using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace EugenePetrenko.DataMigration
{

  public static class XmlUtil
  {
    public static void RemoveSelf(this XmlElement e)
    {
      e.ParentNode.RemoveChild(e);
    }
  }

  public static class Util {

    public static R Apply<T, R>(this T t, Func<T, R> action)
    {
      return action(t);
    }

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
      var doc = LoadXmlDocument(ec, file, true);

      update(doc.DocumentElement);

      SaveDocument(ec, file, doc);
    }

    private static XmlDocument LoadXmlDocument(ErrorsCount ec, string file, bool preserveWhitespace)
    {
      var doc = new XmlDocument();
      doc.PreserveWhitespace = preserveWhitespace;

      ec.Catch("Load " + file,
        () => doc.Load(file),
        e => e.LogAndThrow("Failed to load XML document form " + file));
      return doc;
    }

    private static void SaveDocument(ErrorsCount ec, string xmlFile, XmlDocument doc)
    {
      using (var ms = new MemoryStream())
      {
        ec.Catch("Generating XML for " + xmlFile, () =>
        {
          var settings = new XmlWriterSettings
          {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\r\n",
            NewLineHandling = NewLineHandling.Replace,
            Encoding = new UTF8Encoding(false),
            CheckCharacters = true,
          };

          using (var writer = XmlWriter.Create(ms, settings))
          {
            doc.Save(writer);
          }
        }, e => e.LogAndThrow("Failed to generate XML for {0}. {1}\n{2}", xmlFile, e.Message, e.Exception));

        ec.Catch("Save " + xmlFile, () =>
        {
          using (var w = File.Create(xmlFile))
          {
            var text = Encoding.UTF8.GetString(ms.ToArray()).Trim() + "\r\n";
            var buff = Encoding.UTF8.GetBytes(text);
            w.Write(buff, 0, buff.Length);
          }
        },
          e => e.LogAndThrow("Failed to save XML for " + xmlFile));
      }
    }

    public static void BeautifyXML(ErrorsCount ec, string xmlFile)
    {
      var doc = LoadXmlDocument(ec, xmlFile, false);
      SaveDocument(ec, xmlFile, doc);
    }
  }
}
