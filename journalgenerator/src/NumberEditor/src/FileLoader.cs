using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MIMER.RFC2045;

namespace EugenePetrenko.NumberEditor
{
  public static class FileLoader
  {
    public static string LoadAllFiles(IEnumerable<string> _fileNames)
    {
      var fileNames = _fileNames.OrderBy(x => x).Distinct().ToList();
      var sb = new StringBuilder();
      foreach (var file in fileNames)
      {
        try
        {
          LoadFile(file, sb);
        }
        catch (Exception ee)
        {
          IncludeFileHeader(file, sb);
          IncludeFileContent("xxx", ee.ToString(), sb);
        }
      }

      sb.Append("\r\n\r\n\r\n" + string.Join("\r\n", fileNames) + "\r\n\r\n\r\n");
      return sb.ToString();
    }

    private static bool Extension(this string file, params string[] exts)
    {
      return exts.Any(ext => file.EndsWith(ext, StringComparison.CurrentCultureIgnoreCase));
    }

    private static void LoadFile(string file, StringBuilder sb)
    {
      if (file.Extension(file, ".pdf",".dvi",".doc",".dot",".docx",".eps", ".aux", ".log", ".sty", ".jpg", ".jpeg", ".tex")) return;

      if (file.Extension(".mht"))
      {
        LoadMHT(file, sb);
        return;
      }

      if (file.Extension(".doc",".docx"))
      {
        LoadDOCX(file, sb);
        return;
      }
      
      using (var rdr = new StreamReader(file, Encoding(file)))
      {
        IncludeFileContent(file, rdr.ReadToEnd().Trim(), sb);
      }
    }

    public static void LoadDOCX(string file, StringBuilder sb)
    {
      var copy = file + "auto." + DateTime.Now.Ticks + ".fix.html";

      DOCXConverter.DocToHTML(file, copy);

      LoadFile(copy, sb);
    }


    private static void LoadMHT(string file, StringBuilder sb)
    {
      IncludeFileHeader(file, sb);

      using (var s = File.OpenRead(file))
      {
        Stream ms = s;
        var msg = new MailReader().ReadMimeMessage(ref ms, new BasicEndOfMessageStrategy());

        foreach (var attach in msg.Attachments.notNull())
        {
          if (attach.Type + "/" + attach.SubType == "text/html")
          {
            var subFile = file + "!" + (attach.Name ?? "") + ".html";

            using (var rdr = new StreamReader(new MemoryStream(attach.Data), Encoding(subFile)))
            {
              IncludeFileContent(subFile, rdr.ReadToEnd().Trim(), sb);
            }
          }
        }
      }
    }

    private static void IncludeFileHeader(string file, StringBuilder sb)
    {
      sb.Append("\r\n\r\n----" + file + "\r\n\r\n");
    }

    private static void IncludeFileContent(string file, string text, StringBuilder sb)
    {
      if (file.Contains(".fix."))
      {
        text = HTMLHelpers.FixWordHTML(text);
      }

      if (file.Contains(".bib."))
      {
        text = TEXHelpers.FixTexIntoHTML(text);
      }

      IncludeFileHeader(file, sb);
      sb.AppendLine(text);
    }

    private static Encoding Encoding(string file)
    {
      return file.Contains(".utf8.") ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding("windows-1251");
    }
  }
}
