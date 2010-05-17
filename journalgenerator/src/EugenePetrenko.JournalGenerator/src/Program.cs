using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Antlr.StringTemplate;
using EugenePetrenko.DataModel;
using EugenePetrenko.JournalGenerator.Inforeg;
using EugenePetrenko.RFFI;
using ICSharpCode.SharpZipLib.Zip;
using System.Linq;

namespace EugenePetrenko.JournalGenerator
{
  public class Program
  {
    private static Program myInstance = null;

    public static Program Instance
    {
      get { return myInstance; }
    }

    private readonly List<Language> myLanguages;
    private readonly CommandLineParser myCommandLine;
    private readonly LinkManager myLinkManager;
    private readonly Dictionary<Language, StringTemplateGroup> myTemplates;
    private readonly IJournal myJournal;
    private readonly PdfManager myPdfManager;
    public readonly string DataDir;
    public readonly string DestDir;

    private readonly Hashset<HtmlGenerationContext> myProcessedPages = new Hashset<HtmlGenerationContext>();
    private readonly Queue<HtmlGenerationContext> myQueue = new Queue<HtmlGenerationContext>();

    private readonly Dictionary<string, ConvertToLanguage> myGlobalContext = new Dictionary<string, ConvertToLanguage>();
    private readonly string myTemplatesPath;

    public PdfManager PdfManager
    {
      get { return myPdfManager; }
    }

    public Program(CommandLineParser commandLineParser)
    {
      myInstance = this;
      myCommandLine = commandLineParser;
      string destFile = myCommandLine.GetValue("dest");
      DestDir = destFile;

      try
      {
        FileUtil.SmartDelete(destFile);
      }
      catch
      {
        Console.Out.WriteLine("Umable to clean up output folder {0}", destFile);
      }

      myLinkManager = new LinkManager(myCommandLine.GetValue("url"), destFile);

      myTemplatesPath = Path.GetFullPath(commandLineParser.GetValue("templates"));

      if (!Directory.Exists(myTemplatesPath))
      {
        Console.Out.WriteLine("Failed to find templates dir");
        return;
      }
      string data = Path.GetFullPath(commandLineParser.GetValue("data"));
      DataDir = data;
      string version = DateTime.Now.ToString("yyyy-MM-dd");

      BackUp(data, Path.Combine(destFile, "backup\\data-" + version + ".zip"), x=> { });
      BackUp(myTemplatesPath, Path.Combine(destFile, "backup\\templates-" + version + ".zip"), x=> FileUtil.SmartDelete(Path.Combine(x, "shared/books")));

      myJournal = XmlDataLoader.Parse(data);
      myPdfManager = new PdfManager(myLinkManager, Path.Combine(Path.GetDirectoryName(myTemplatesPath), "pdf"));

      Console.Out.WriteLine("Template path = {0}, exists = [{1}]", myTemplatesPath, Directory.Exists(myTemplatesPath));

      myTemplates = new Dictionary<Language, StringTemplateGroup>();

      FileUtil.Copy(Path.Combine(myTemplatesPath, "shared"), destFile);

      myLanguages = new List<Language>();
      foreach (Language lang in Enum.GetValues(typeof (Language)))
      {
        myLanguages.Add(lang);
        string tpath = Path.Combine(myTemplatesPath, lang.ToString());

        Console.Out.WriteLine("Loading template for lang {0} from {1}", lang, tpath);

        //TODO: Switch to unicode templates
        var loader = new FileSystemTemplateLoader(tpath, Encoding.UTF8);
        var group = new StringTemplateGroup("journal_" + lang, loader);        
        group.RegisterAttributeRenderer(typeof (DateTime), new DateRenderer());
        myTemplates[lang] = group;
      }
    }

    private static void BackUp(string folder, string output, Action<string> removeNotNeeded)
    {
      string backup = folder + "qqq";
      FileUtil.Copy(folder, backup);

      removeNotNeeded(backup);

      string dir = Path.GetDirectoryName(output);
      if (!Directory.Exists(dir))
        Directory.CreateDirectory(dir);

      var zip = new FastZip {CreateEmptyDirectories = false};
      zip.CreateZip(output, backup, true, null);

      FileUtil.SmartDelete(backup);
    }

    public void GeneratePage(HtmlGenerationContext ctxp)
    {
      var ctx = new FileHtmlContext(myLinkManager, ctxp);
      Console.Out.WriteLine("\r\nGeneratign page {1}: {0}...", ctx, ctx.TemplateName);

      foreach (Language language in myLanguages)
      {
        FileLanguageGenerationContext context = ctx.LanguageContext(language);
        context.GeneratePageToFile();
      }

      Console.Out.WriteLine("Done\r\n");
    }

    public void AddPredefinedProperties(IDictionary dic, Language lang, LinkTemplate page)
    {
      foreach (KeyValuePair<string, ConvertToLanguage> pair in myGlobalContext)
      {
        if (!dic.Contains(pair.Key))
          dic.Add(pair.Key, pair.Value(lang, page));
      }
    }

    public void BuildPages()
    {
      myQueue.Enqueue(new JournalContentsContext(myLinkManager, myJournal));
      myQueue.Enqueue(new NewsGenerationContext(myJournal, myLinkManager));
      myQueue.Enqueue(new BooksGenerationContext(myJournal, myLinkManager));

      while (myQueue.Count > 0)
      {
        HtmlGenerationContext ctx = myQueue.Dequeue();
        if (myProcessedPages.Contains(ctx))
          continue;

        GeneratePage(ctx);

        myProcessedPages.Add(ctx);
      }

      myPdfManager.CopyFiles();
    }

    public void BuildRFFI()
    {
      string rffi = Path.Combine(DestDir, "RFFI");
      Directory.CreateDirectory(rffi);

      foreach (INumber number in myJournal.Numbers)
      {
        var copy = myPdfManager.GetSubCopier();
        string dir = Path.Combine(rffi, number.Year + "-" + number.Number);
        Directory.CreateDirectory(dir);

        var num = new RFFIJournalNumber(new RFFIIssue(number));
        XmlDocument doc = XmlAttributeProcessor.Build(num);


        string file = Path.Combine(dir, string.Format("{0}-{1}_unicode.xml", number.Year, number.Number));
        using (var stream = new StreamWriter(file, false, Encoding.Unicode))
          doc.Save(stream);

        num.RegisterPdfDownload((art, fileName)=>copy.RegisterPdfWithName(art, Path.Combine(dir, fileName)));

        copy.CopyFiles();
        
        var z = new FastZip();
        z.CreateZip(dir + ".zip", dir, true, null);
      }      
    }

    public void BuildInforeg(string numYear)
    {
      var number = myJournal.Numbers.Where(x => numYear == string.Format("{0}-{1}", x.Number, x.Year)).Single();
      BuildInfoRegData(number);
    }

    public void BuildInforegNumbers(int count)
    {
      var col = myJournal.Numbers.OrderBy(x => -x.IntYear*1000 - x.IntNumber).Take(count);
      foreach (var number in col)
      {
        BuildInfoRegData(number);
      }
    }

    private void BuildInfoRegData(INumber number)
    {
      var folder = Path.Combine(DestDir, string.Format("inforeg-{0}-{1}", number.Year, number.Number));
      new InforegArticlesReport(number, folder, myTemplatesPath, myPdfManager).GenerateReport();
    }

    public void AddPage(HtmlGenerationContext page)
    {
      myQueue.Enqueue(page);
    }

    public void AppendGlobalContext(string name, HtmlGenerationContext ctx)
    {
      myGlobalContext.Add(name, ctx.LinkTemplate.ToLink);
    }

    public Dictionary<Language, StringTemplateGroup> Templates
    {
      get { return myTemplates; }
    }

    private static int Main(string[] _args)
    {
//      string[] args = new string[] { @"/url=http://diff.neva.ru/j/", string.Format(@"/dest=E:\Projects\journalGenerator\release\{0}", DateTime.Now.ToString("yyyy-MM-dd--hh-mm-ss")) };
      if (_args.Length == 0)
      {
        Usage();
        return -1;
      }
//      string[] args = new string[] {@"/url=file:\\\c:\tmp\", @"/dest=c:\tmp\"};
      var parser = new CommandLineParser(_args);
      var program = new Program(parser);
      
      program.BuildPages();
      if (parser.HasKey("rffi"))
      {
        program.BuildRFFI();
      }

      if (parser.HasKey("inforeg"))
      {
        foreach (var yn in parser.GetValue("inforeg").Split(','))
        {
          program.BuildInforeg(yn);
        }
      }

      if (parser.HasKey("inforegs"))
      {
        program.BuildInforegNumbers(int.Parse(parser.GetValue("inforegs")));
      }

      return 0;
    }

    private static void Usage()
    {
      Console.Out.WriteLine("Usage:");
      Console.Out.WriteLine("  prog.exe /url=<base_url> /dest=<path to get> /templates=<path to templates> /data=<path to data> [/rffi]");
      Console.Out.WriteLine("  where:");
      Console.Out.WriteLine("    /rffi - to build rffi export xml files");
      Console.Out.WriteLine("    /inforeg=<num>-<year> - to build inforeg files");
    }
  }
}