using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator
{
  public class PdfManager
  {
    private readonly Dictionary<PdfLink, string> myCopyFiles = new Dictionary<PdfLink, string>();
    private readonly Dictionary<PdfLink, IArticle> myPdfLinksToArticle = new Dictionary<PdfLink, IArticle>();
    private readonly LinkManager myLinkManager;
    private readonly string myPdfStorePath;

    public PdfManager(LinkManager linkManager, string pdfStorePath)
    {
      myLinkManager = linkManager;
      myPdfStorePath = pdfStorePath;
    }

    public PdfLink RegisterPdf(IArticle article, Language lang, LinkTemplate page)
    {
      IArticleInfo language = article.ForLanguage(LanguageUtil.Convert(lang));
      string name = PdfName(language);

      Link pageLink = page.ToLink(lang, null);

      PdfLink link = new PdfLink(myLinkManager, name + ".pdf", pageLink);
      IArticle tmp;
      while (myPdfLinksToArticle.TryGetValue(link, out tmp) && tmp != article)
      {
        name += "j";
        link = new PdfLink(myLinkManager, name + ".pdf", pageLink);
      }

      string pdfFIle = Path.Combine(myPdfStorePath, language.Pdf);
      if (!File.Exists(pdfFIle))
        Console.Error.WriteLine("Filed to get file {0} for acticle {1}", pdfFIle, article);

      myCopyFiles[link] = pdfFIle;
      myPdfLinksToArticle[link] = article;
      return link;
    }

    private static string PdfName(IArticleInfo art)
    {
      StringBuilder sb = new StringBuilder();
      foreach (char c in Path.GetFileNameWithoutExtension(art.Pdf))
      {
        if (char.IsLetterOrDigit(c))
        {
          sb.Append(char.ToLower(c));
        }
        else
        {
          sb.Append('_');
        }
      }
      return Path.GetFileNameWithoutExtension(sb.ToString());
    }

    public void CopyFiles()
    {
      Console.Out.WriteLine("Copy pdf files to the dest folder");

      foreach (KeyValuePair<PdfLink, string> file in myCopyFiles)
      {
        string dest = file.Key.DestFile;
        string dir = Path.GetDirectoryName(dest);
        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);

        if (!File.Exists(file.Value))
          Console.Error.WriteLine("File {0} was not found", file.Value);
        else
          File.Copy(file.Value, dest);
      }
    }
  }
}