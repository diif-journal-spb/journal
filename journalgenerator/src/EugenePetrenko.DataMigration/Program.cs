using System;
using System.IO;
using EugenePetrenko.JournalGenerator;

namespace EugenePetrenko.DataMigration
{
  internal class Program
  {
    private static int Main(string[] args)
    {
      try
      {
        MainImpl(args);
      }
      catch (Exception e)
      {
        Console.Out.WriteLine("Failed. " + e.Message);
        Console.Out.WriteLine(e);
        return 1;
      }
      return 0;
    }

    private static void MainImpl(string[] args)
    {
      var cmd = new CommandLineParser(args);

      Console.Out.WriteLine("Automatic data migration");
      Console.Out.WriteLine("<app>.exe /data=<data> /pdf=<pdf>");

      var data = Path.GetFullPath(cmd.GetValue("data"));
      var pdf = Path.GetFullPath(cmd.GetValue("pdf"));

      Console.Out.WriteLine("Using data directory: {0}", data);
      Console.Out.WriteLine("Using pdf directory: {0}", pdf);


      //Dummy re-format files
      Util.ProcessFiles(data, "*.orgs", file => Util.UpdateXmlDocument(file, el => { }));
      Util.ProcessFiles(data, "*.authors", file => Util.UpdateXmlDocument(file, el => { }));
      Util.ProcessFiles(data, "*.authors", file => Util.UpdateXmlDocument(file, el => { }));

      //new ExtractAuthorsToAdditionalFiles(data).Process();
    }
  }
}
