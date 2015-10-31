using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using EugenePetrenko.DataModel;
using EugenePetrenko.RFFI;

namespace EugenePetrenko.JournalGenerator
{
  public class PdfFileCopier
  {
    private readonly Dictionary<string, string> myCopyFiles = new Dictionary<string, string>();
    private readonly string myPdfStorePath;

    public PdfFileCopier(string myPdfStorePath)
    {
      this.myPdfStorePath = myPdfStorePath;
    }

    public void RegisterPdfWithName(IArticleInfo article, string relPath)
    {
      string pdfFIle = Path.Combine(myPdfStorePath, article.Pdf);
      if (!File.Exists(pdfFIle))
        Console.Error.WriteLine("Filed to get file {0} for acticle {1}", pdfFIle, article);

      myCopyFiles[relPath] = pdfFIle;
    }

    public void CopyFiles()
    {
      Console.Out.WriteLine("Copy pdf files to the dest folder");

      foreach (KeyValuePair<string, string> file in myCopyFiles)
      {
        string dest = file.Key;
        string dir = Path.GetDirectoryName(dest);
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);

        if (!File.Exists(file.Value))
          Console.Error.WriteLine("File {0} was not found", file.Value);
        else
          File.Copy(file.Value, dest);
      }

      myCopyFiles.Clear();
    }

    public PdfFileCopier GetSubCopier()
    {
      return new PdfFileCopier(myPdfStorePath);
    }
  }

  public class PdfManager : PdfFileCopier, IPdfTextManager
  {
    /// <summary>
    /// Dest -> Source
    /// </summary>
    
    private readonly Dictionary<PdfLink, IArticle> myPdfLinksToArticle = new Dictionary<PdfLink, IArticle>();
    private readonly LinkManager myLinkManager;
    private readonly string myPdfStorePath;

    public PdfManager(LinkManager linkManager, string pdfStorePath) : base(pdfStorePath)
    {
      myLinkManager = linkManager;
      myPdfStorePath = pdfStorePath;
    }

    public string PdfText(IArticleInfo article)
    {
      var file = Path.Combine(myPdfStorePath + ".text", Path.GetFileNameWithoutExtension(article.Pdf) + ".text");
      using (var t = new StreamReader(File.OpenRead(file), Encoding.UTF8))
      {
          var text = t.ReadToEnd();
          text = new string(text.Select(x => XmlConvert.IsXmlChar(x) ? x : ' ').ToArray());
          text = Regex.Replace(text, @"\s+", " ");
          return text;
      }
    }

    public PdfLink RegisterPdf(IArticle article, Language lang, LinkTemplate page)
    {
      IArticleInfo language = article.ForLanguage(LanguageUtil.Convert(lang));
      string name = PdfName(language);

      Link pageLink = page.ToLink(lang, null);

      var link = new PdfLink(myLinkManager, name + ".pdf", pageLink);
      IArticle tmp;
      while (myPdfLinksToArticle.TryGetValue(link, out tmp) && tmp != article)
      {
        name += "j";
        link = new PdfLink(myLinkManager, name + ".pdf", pageLink);
      }

      RegisterPdfWithName(language, link.DestFile);
      myPdfLinksToArticle[link] = article;
      return link;
    }

    private static string PdfName(IArticleInfo art)
    {
      var sb = new StringBuilder();
      foreach (char c in Path.GetFileNameWithoutExtension(art.Pdf))
      {
        sb.Append(char.IsLetterOrDigit(c) ? char.ToLower(c) : '_');
      }
      return Path.GetFileNameWithoutExtension(sb.ToString());
    }
  }
}