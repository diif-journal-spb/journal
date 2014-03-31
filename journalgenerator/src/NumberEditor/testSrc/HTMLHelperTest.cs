using System;
using NUnit.Framework;

namespace EugenePetrenko.NumberEditor
{
  [TestFixture]
  public class HTMLHelperTest
  {

    [Test]
    public void test_reference_html()
    {
      var input = @"<p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'>29. Ushho
D.S., Ushho A.D. [About existing of limiting cycles and line particular
integrals of cubic differential systems on plane]. <i style='mso-bidi-font-style: 
normal'>Trudy FOR A</i>, 2004; (9):20-24. <span class=GramE>(In Russ.)</span><o:p></o:p></span></p >";


      var gold = HTMLHelpers.FixWordHTML(input);
      Console.Out.WriteLine(gold);
      Assert.AreEqual(
        "29. Ushho D.S., Ushho A.D. [About existing of limiting cycles and line particular integrals of cubic differential systems on plane]. <i>Trudy FOR A</i>, 2004; (9):20-24. (In Russ.)", 
        gold);
    }

    [Test]
    public void test_reference_html2()
    {
      var input = @"<p class=MsoNormal><span lang=EN-US style='mso-ansi-language:EN-US'>2. Wilson H.R.
Spikes, decisions and actions. <span class=GramE>The dynamical foundations of
neuroscience.</span> <st1:State w:st=""on"">New York</st1:State>: <st1:place
w:st=""on""><st1:PlaceName w:st=""on"">Oxford</st1:PlaceName> <st1:PlaceType w:st=""on"">University</st1:PlaceType></st1:place>
Press, 2005. <span class=GramE>307 p.</span><o:p></o:p></span></p>";

      var gold = HTMLHelpers.FixWordHTML(input).Trim();
      
      Console.Out.WriteLine(gold);
      Assert.AreEqual(
        "2. Wilson H.R. Spikes, decisions and actions. The dynamical foundations of neuroscience. New York: Oxford University Press, 2005. 307 p.", 
        gold);
    }

    [Test]
    public void test_reference_html3()
    {
      var input = @"<p class=MsoPlainText><span lang=EN-US style='font-family:""Courier New"";
mso-ansi-language:EN-US'>1. T.A. Burton, Stability and periodic solutions of
ordinary and functional differential equations, Mathematics in Science and
Engineering<span class=GramE>,178</span>. Academic Press, Inc., Orlando, FL,
1985.<o:p></o:p></span></p>
";

      var gold = HTMLHelpers.FixWordHTML(input).Trim();
      
      Console.Out.WriteLine(gold);
      Assert.AreEqual(
        "1. T.A. Burton, Stability and periodic solutions of ordinary and functional differential equations, Mathematics in Science and Engineering,178. Academic Press, Inc., Orlando, FL, 1985.", 
        gold);
    }
  }
}