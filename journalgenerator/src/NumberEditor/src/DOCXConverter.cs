using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace EugenePetrenko.NumberEditor
{
  public static class DOCXConverter
  {
    public static void DocToHTML(string file, string copy)
    {
      Extensions.ExecuteUnderSTA(() =>
      {
        DocToHTMLImpl(file, copy);

        for (int i = 0; i < 20; i++)
        {
          GC.Collect();
          GC.WaitForPendingFinalizers();

          try
          {
            //File is locked if Word object is still around
            using (File.Open(copy, FileMode.Append))
            {
              return 42;
            }
          }
          catch
          {
            //NOP. Have to wait for Word to exit
          }

          Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }
        throw new Exception("Failed to wait for Word to exit");
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
          wordFile.SaveAs(FileName: copy, FileFormat: /*wdFormatHTML*/8);
        }
        finally
        {
          wordFile.Close(SaveChanges: /*wdSaveChanges*/ -1);
          Call(() => { Marshal.ReleaseComObject(wordFile); });
        }
        foreach (dynamic doc in wordObject.Documents)
        {
          doc.Close(SaveChanges: /*wdDoNotSaveChanges*/ 0);
        }
      }
      finally
      {
        wordObject.Quit(SaveChanges: /*wdDoNotSaveChanges*/ 0);
        Call(() => { Marshal.ReleaseComObject(wordObject); });
      }
    }

    private static void Call(Action a)
    {
      try
      {
        a();
      }
      catch
      {
        //NOP
      }
    }
  }
}
