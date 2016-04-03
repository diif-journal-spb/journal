using System.IO;
using NUnit.Framework;

namespace EugenePetrenko.NumberEditor
{
  [TestFixture]
  public class DOCXConverterTest
  {
    [Test, RequiresSTA]
    public void TestConvertSTA()
    {
      TestFiles.WithResource("docx.docx", (dir, file) =>
      {
        var html = file + ".html";

        DOCXConverter.DocToHTML(file, html);

        //Make sure file is not locked my a running MSWord
        using (var s = File.OpenText(html))
        {
          s.ReadToEnd();
        }
      });
    }

    [Test, RequiresMTA]
    public void TestConvertMTA()
    {
      TestFiles.WithResource("docx.docx", (dir, file) =>
      {
        var html = file + ".html";

        DOCXConverter.DocToHTML(file, html);

        //Make sure file is not locked my a running MSWord
        File.OpenText(html).ReadToEnd();
      });
    }
  }
}
