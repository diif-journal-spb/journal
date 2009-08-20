using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr.StringTemplate;
using EugenePetrenko.DataModel;

namespace EugenePetrenko.JournalGenerator.Inforeg
{
  public class InforegArticlesReport
  {
    private readonly INumber myNumber;
    private readonly string myTemplatesFolder;
    private readonly string myDestFolder;
    private readonly PdfManager myPdfManager;

    public InforegArticlesReport(INumber number, string destFolder, string templatesFolder, PdfManager pdfManager)
    {
      myNumber = number;
      myPdfManager = pdfManager;
      myTemplatesFolder = templatesFolder;
      myDestFolder = destFolder;
    }

    public void GenerateReport()
    {
      if (!Directory.Exists(myDestFolder))
      {
        Directory.CreateDirectory(myDestFolder);
      }

      var g = new StringTemplateGroup("inforeg.ru", Path.Combine(myTemplatesFolder, "inforeg"));
      var template = g.LookupTemplate("articles");

      var model = BuildModel();

      template.Attributes = model.GetProperties();
      File.WriteAllText(Path.Combine(myDestFolder, "index.htm"), template.ToString());

      myPdfManager.CopyFiles();
    }

    private Model BuildModel()
    {
      int myCount = 0;
      var aaa = new List<Article>();
      foreach (var numberSection in myNumber.Sections)
      {
        foreach (var info in numberSection.Articles.Select(x => x.ForLanguage(JournalLanguage.RU)))
        {
          processArticle(ref myCount, aaa, info);
        }
      }

      return new Model
               {
                 Number = myNumber.Number,
                 Year = myNumber.Year,
                 Articles = aaa
               };
    }

    private void processArticle(ref int myCount, ICollection<Article> aaa, IArticleInfo info)
    {
      var url = ++myCount + ".pdf";
      myPdfManager.RegisterPdfWithName(info, Path.Combine(myDestFolder, url));
      aaa.Add(new Article
                {
                  Authors = String.Join(", ", info.Authors.Select(x=>x.FirstName + " " + x.MiddleName + " " + x.LastName).ToArray()),
                  Name = info.Title,
                  Url = url
                });
    }

    private class Model
    {
      public string Number { get; set; }
      public string Year { get; set; }

      public IEnumerable<Article> Articles { get; set; }
    }

    private class Article
    {
      public string Authors { get; set; }
      public string Name { get; set; }
      public string Url { get; set; }
    }
  }
}