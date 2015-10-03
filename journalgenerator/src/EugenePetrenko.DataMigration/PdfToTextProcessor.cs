using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Path = System.IO.Path;

namespace EugenePetrenko.DataMigration
{
  public static class PdfToTextProcessor
  {
    public static void Process(ErrorsCount ec, string pdfDir)
    {
      Console.Out.WriteLine("======");
      var homeDir = pdfDir + ".text";

      if (!Directory.Exists(homeDir)) {
        Directory.CreateDirectory(homeDir);
      }

      Util.ProcessFiles(ec, pdfDir, "*.pdf", pdfFile =>
      {
        var marker = Path.Combine(homeDir, Path.GetFileNameWithoutExtension(pdfFile) + ".text");
        if (File.Exists(marker) && File.GetLastWriteTime(marker) >= File.GetLastWriteTime(pdfFile)) return;

        var text = new StringBuilder();
        using (var pdfReader = new PdfReader(pdfFile))
        {
          Console.Out.WriteLine("Processing {0}, pages {1}", pdfFile, pdfReader.NumberOfPages);

          for (int page = 1; page <= pdfReader.NumberOfPages; page++)
          {
            try
            {
              ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

              string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

              /* http://stackoverflow.com/questions/2550796/reading-pdf-content-with-itextsharp-dll-in-vb-net-or-c-sharp
             * Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)))
             */

              text.Append(currentText);
            }
            catch
            {
              text.Append("\n");
            }
          }

          var actualText = text.ToString();
          actualText = Regex.Replace(actualText,
            @"Электронный журнал.\s+http://www.math.spbu.ru/di..?journal\s+\d+Дифференциальные уравнения и процессы управления,N.\s+\d+,\s+\d+",
            "");
          actualText = Regex.Replace(actualText,
            @"Электронный журнал.\s+\d+Дифференциальные уравнения и процессы управления,N.\s+\d+,\s+\d+",
            "");
          actualText = Regex.Replace(actualText,
            @"Электронный журнал.\s+http://www.neva.ru/journal\s+\d+Дифференциальные уравнения и процессы управления,\s+N.\s+\d+,\s+\d+",
            "");
          actualText = Regex.Replace(actualText,
            @"Electronic Journal.\s+http://www.math.spbu.ru/di..?journals+\d+Di..?erential Equations and Control Processes,s+Ns+d+,\s+d+",
            "");
          actualText = Regex.Replace(actualText,
            @"Electronic Journal.\s+http://www.neva.ru/journal+\d+Di..?erential Equations and Control Processes,s+Ns+d+,\s+d+",
            "");
          actualText = Regex.Replace(actualText,
            @"Electronic Journal.\s+http://www.neva.ru/journal,\s+http://www.math.spbu.ru/user/di®journal/?\s+\d+Di..?erential Equations and Control Processes,\s+N\s+\d+,\s+\d+",
            "");
          actualText = Regex.Replace(actualText,
            @"Electronic Journal.\s+\d+Differential Equations and Control Processes,\s+N\s+\d+,\s+\d+",
            "");
          actualText = Regex.Replace(actualText,
            @"http://www.neva.ru/journal",
            "");
          actualText = actualText.Replace("®", "ff");
          actualText = actualText.Replace("ﬀ", "ff");
          actualText = actualText.Replace("", "ff");
          actualText = new string(actualText.ToCharArray().Select(x=> !XmlConvert.IsXmlChar(x) ? ' ' : x).ToArray());

          using (var w = new StreamWriter(File.Create(marker), Encoding.UTF8))
          {
            w.WriteLine(actualText);
          }
        }
      });
    }
  }
}