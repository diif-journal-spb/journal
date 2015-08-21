using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using EugenePetrenko.JournalGenerator;

namespace EugenePetrenko.DataMigration
{
  public static class ArticlesInferPages
  {
    public static void Process(ErrorsCount ec, string data, string pdf)
    {
      Util.ProcessFiles(ec, data, "*.number", file => Util.UpdateXmlDocument(ec, file, el =>
      {
        if (el.Name != "number") throw new Exception("<number> element is expected as root");

        int prevNumberLastPage = 0;
        el.SelectNodes("article").Cast<XmlElement>().ForEach(ec, artile =>
        {
          var First = new Hashset<int>();
          var Last = new Hashset<int>();

          artile.SelectNodes("articleInfo").Cast<XmlElement>().ForEach(ec, info =>
          {
            int? FirstPage = null;
            int? LastPage = null;

            var FirstPageText = info.GetAttribute("FirstPage");
            var LastPageText = info.GetAttribute("LastPage");

            if (FirstPageText != "" && FirstPageText != "0")
            {
              FirstPage = int.Parse(FirstPageText);
              First.Add(FirstPage.Value);
            }

            if (LastPageText != "" && LastPageText != "0")
            {
              LastPage = int.Parse(LastPageText);
              Last.Add(LastPage.Value);
            }

            var pdfFileNode = info.SelectSingleNode("pdf/text()");
            if (pdfFileNode == null) throw new Exception("No PDF for article");
            var pdfFile = pdfFileNode.Value.Trim();

            var pdfPath = Path.Combine(pdf, pdfFile);
            if (!File.Exists(pdfPath)) throw new Exception("Failed to find PDF: " + pdfFile);


            if (FirstPage != null && LastPage != null)
            {
              if (FirstPage > LastPage) throw new Exception("Invalid pages: " + FirstPage + " => " + LastPage);
              return;
            }

            var pagesInPdf = getNumberOfPdfPages(pdfPath);
            Console.Out.WriteLine("Pages: " + pagesInPdf + " in " + pdfFile);

            var firstPageActual = FirstPage.GetValueOrDefault(prevNumberLastPage + 1);
            var lastPageActual = LastPage.GetValueOrDefault(prevNumberLastPage + pagesInPdf);
            
            First.Add(firstPageActual);
            Last.Add(lastPageActual);

            info.SetAttribute("FirstPage", "" + firstPageActual);
            info.SetAttribute("LastPage", "" + lastPageActual);
          });

          if (First.Count != 1) throw new Exception("Invalid FirstPage values: " + string.Join(", ",  First.Values.OrderBy(x=>x)));
          if (Last.Count != 1) throw new Exception("Invalid LastPage values: " + string.Join(", ", Last.Values.OrderBy(x=>x)));

          prevNumberLastPage = Last.Values.Max();
        });
      }));
    }

    private static int getNumberOfPdfPages(string fileName)
    {
      // http://stackoverflow.com/questions/320281/determine-number-of-pages-in-a-pdf-file
      using (var sr = new StreamReader(File.OpenRead(fileName)))
      {
        Regex regex = new Regex(@"/Type\s*/Page[^s]");
        MatchCollection matches = regex.Matches(sr.ReadToEnd());

        return matches.Count;
      }
    }
  }
}
