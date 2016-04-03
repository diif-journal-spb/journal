using System;
using System.IO;

namespace EugenePetrenko.NumberEditor
{
  public static class TestFiles
  {
    public static void WithTemporaryDirectory(Action<string> action)
    {
      string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(tempDirectory);

      try
      {
        action(tempDirectory);
      }
      finally
      {
        Directory.Delete(tempDirectory, true);
      }
    }


    public static void WithResource(string res, Action<string, string> action)
    {
      WithTemporaryDirectory(dir =>
      {
        var target = Path.Combine(dir, res);

        using(var t = File.OpenWrite(target))
        using (var s = typeof(TestFiles).Assembly.GetManifestResourceStream("EugenePetrenko.NumberEditor.testData." + res))
        {
          s.CopyTo(t);
        }
      });
    }
  }
}
