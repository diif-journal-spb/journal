using System;
using System.IO;
using System.Text;

namespace EugenePetrenko.DataMigration
{
  public static class PfdToTextValidateProcessor
  {
    public static void Process(ErrorsCount ec, string pdfDir)
    {
      Console.Out.WriteLine("======");
      var homeDir = pdfDir + ".text";

      if (!Directory.Exists(homeDir))
      {
        Directory.CreateDirectory(homeDir);
      }

      Util.ProcessFiles(ec, pdfDir, "*.pdf", pdfFile =>
      {
        var marker = Path.Combine(homeDir, Path.GetFileNameWithoutExtension(pdfFile) + ".text");
        if (!File.Exists(marker))
        {
          ec.Error("No text file for: {0}", Path.GetFileName(pdfFile));
          return;
        }

        string actualText;
        using (var w = new StreamReader(File.OpenRead(marker), Encoding.UTF8))
        {
          actualText = w.ReadToEnd();
        }

        int totalChars = 0;
        int letters = 0;
        foreach (var _ch in actualText)
        {
          totalChars++;
          
          //TODO

        }
        
      });
    }
  }
}