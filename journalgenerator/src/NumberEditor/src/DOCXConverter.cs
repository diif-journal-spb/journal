using System;
using System.Runtime.InteropServices;

namespace EugenePetrenko.NumberEditor
{
  public static class DOCXConverter
  {
    public static void DocToHTML(string file, string copy)
    {
      Extensions.ExecuteUnderSTA(() =>
      {
        DocToHTMLImpl(file, copy);
        return 42;
      });
    }

    private static void DocToHTMLImpl(string file, string copy)
    {
      var wordType = Type.GetTypeFromProgID("Word.Application");
      dynamic wordObject = Activator.CreateInstance(wordType);
      try
      {
        dynamic wordFile = wordObject.Documents.Open(file);
        try
        {
          const int wdFormatHTML = 8;
          wordFile.SaveAs(copy, wdFormatHTML);
        }
        finally
        {
          Marshal.ReleaseComObject(wordFile);
        }
      }
      finally
      {
        wordObject.Quit();
        Marshal.ReleaseComObject(wordObject);
      }
    }
  }
}
