using System;
using System.IO;

namespace EugenePetrenko.JournalGenerator
{
  public static class FileUtil
  {
    public static void Copy(string fromDir, string toDir)
    {
      if (fromDir.EndsWith(".svn"))
        return;

      if (File.Exists(fromDir))
      {
        File.Copy(fromDir, toDir, true);
        return;
      }

      if (!Directory.Exists(toDir))
        Directory.CreateDirectory(toDir);      

      foreach (string file in Directory.GetFiles(fromDir))
      {
        string destFile = Path.Combine(toDir, Path.GetFileName(file));
        if (!File.Exists(destFile))
        {
          File.Copy(file, destFile);
        }
        else
        {
          Console.Out.WriteLine("File allready exists {0} -> {1}", file, destFile);
        }
      }

      foreach (string dir in Directory.GetDirectories(fromDir))
      {
        Copy(dir, Path.Combine(toDir, Path.GetFileName(dir)));
      }
    }

    public static void SmartDelete(string fromDir)
    {
      if (!Directory.Exists(fromDir))
        return;

      foreach (string file in Directory.GetFiles(fromDir))
      {
        File.SetAttributes(file, FileAttributes.Normal);
        File.Delete(file);
      }

      foreach (string dir in Directory.GetDirectories(fromDir))
      {
        SmartDelete(dir);
      }

      if (Directory.GetFiles(fromDir).Length == 0)
        Directory.Delete(fromDir);
    }
  }
}