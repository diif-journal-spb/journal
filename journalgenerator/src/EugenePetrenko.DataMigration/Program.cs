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
        var cmd = new CommandLineParser(args);
        var ec = new ErrorsCount();
        MainImpl(cmd, ec);

        Console.Out.WriteLine("");
        Console.Out.WriteLine("Errors: {0}", ec.TotalErrors);

        return ec.TotalErrors == 0 ? 0 : 2;
      }
      catch (Exception e)
      {
        Console.Out.WriteLine("Failed. " + e.Message);
        Console.Out.WriteLine(e);
        return 1;
      }
    }

    private static void MainImpl(CommandLineParser cmd, ErrorsCount ec)
    {
      Console.Out.WriteLine("Automatic data migration");
      Console.Out.WriteLine("<app>.exe /data=<data> /pdf=<pdf>");

      var data = Path.GetFullPath(cmd.GetValue("data"));
      var pdf = Path.GetFullPath(cmd.GetValue("pdf"));

      Console.Out.WriteLine("Using data directory: {0}", data);
      Console.Out.WriteLine("Using pdf directory: {0}", pdf);


      //Dummy re-format files
      Util.ProcessFiles(ec, data, "*.orgs", file => Util.UpdateXmlDocument(ec, file, el => { }));
      Util.ProcessFiles(ec, data, "*.authors", file => Util.UpdateXmlDocument(ec, file, el => { }));
      Util.ProcessFiles(ec, data, "*.number", file => Util.UpdateXmlDocument(ec, file, el => { }));


      //Infer/check first-last pages of all articles, 
      //ensure PDFs are available
      ArticlesInferPages.Process(ec, data, pdf);


      //make sure all authors are declared outside .number files
      ExtractAuthorsToAdditionalFiles.Process(ec, data);

      //make sure all author organizations are included
      AuthorOrganizationProcessor.Process(ec, data);

    }
  }
}
