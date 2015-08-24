using System;
using System.IO;
using System.Text;
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
        if (File.Exists(marker)) return;

        var text = new StringBuilder();
        using (var pdfReader = new PdfReader(pdfFile))
        {
          Console.Out.WriteLine("Processing {0}, pages {1}", pdfFile, pdfReader.NumberOfPages);

          for (int page = 1; page <= pdfReader.NumberOfPages; page++)
          {
            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
            
            string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

            /* http://stackoverflow.com/questions/2550796/reading-pdf-content-with-itextsharp-dll-in-vb-net-or-c-sharp
             * Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)))
             */

            text.Append(currentText);
          }

          using (var w = new StreamWriter(File.Create(marker), Encoding.UTF8))
          {
            w.WriteLine(text);
          }
        }
      });
    }
  }
}