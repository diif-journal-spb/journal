using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace patcher
{
  class Program
  {
    static void Main(string[] args)
    {
      if (args.Length != 3) {
        Console.Out.WriteLine("Usage: file A B Output");
        return;
      }
      string dir1 = Path.GetFullPath(args[0]);
      string dir2 = Path.GetFullPath(args[1]);
      string o = Path.GetFullPath(args[2]);
      
      Dictionary<string, string> d1 = new Dictionary<string, string>();
      buildPatch(dir1, "", d1);
      
      Dictionary<string, string> d2 = new Dictionary<string, string>();      
      buildPatch(dir2, "", d2);
      
      List<string> dk1 = new List<string>(d1.Keys);
      List<string> dk2 = new List<string>(d2.Keys);
      
      
      if (Directory.Exists(o))
        Directory.Delete(o, true);

      Directory.CreateDirectory(o);

      foreach (string fromFile in dk1)
      {
        string h2;
        if (d2.TryGetValue(fromFile, out h2) && d1[fromFile] != h2)
        {
          WriteFile(dir2, fromFile, o);          
        } else
        {
          //File deleted. Just do nothing
        }
      }

      foreach (string toFile in dk2)
      {
        if (!dk1.Contains(toFile))
        {
          WriteFile(dir2, toFile, o);
        }
      }      
    }
    
    private static void WriteFile(string bs, string file, string to)
    {
      string com = Path.Combine(to, file);
      string pcom = Path.GetDirectoryName(com);


      if (!Directory.Exists(pcom))
        Directory.CreateDirectory(pcom);

      File.Copy(Path.Combine(bs, file), com);      
    } 
    
    private static void buildPatch(string file, string prefix, Dictionary<string,string> o)
    {
      foreach (string f in Directory.GetFiles(file))
      {
        string name = prefix  + Path.GetFileName(f);
        o[name] = getFileSignature(f);                
      }
      foreach (string directory in Directory.GetDirectories(file))
      {
        buildPatch(directory, prefix + Path.GetFileName(directory) + @"\", o);
      }      
    }
    
    private static string getFileSignature(string file)
    {
      MD5CryptoServiceProvider prov = new MD5CryptoServiceProvider();
      byte[] hash;
      using (Stream s = File.Open(file, FileMode.Open))
        hash = prov.ComputeHash(s);

      return byteToString(hash);
    }
    
    private static string byteToString(byte[] b)
    {
      StringBuilder sb = new StringBuilder();
      sb.Append(b.Length);
      foreach (byte bb in b)
      {
        sb.Append("_").Append(bb).Append("_");
      }
      return sb.ToString();
    }    
  }
}
