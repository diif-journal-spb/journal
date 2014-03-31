using System;
using NUnit.Framework;

namespace EugenePetrenko.NumberEditor
{

  [TestFixture]
  public class TEXHelpersTest
  {
    [Test]
    public void test_replaces_bibitems()
    {
      var input = @"\bibitem{AhAb07}
K.~Ahnert and M.~Abel.
\newblock Numerical differentiation of experimental data: local versus global
  methods.
\newblock {\em Computer Physics Communications}, 177:764–774, 2007.
";

      var gold = TEXHelpers.FixTexIntoHTML(input);

      Console.Out.WriteLine(gold);

      Assert.AreEqual("K.Ahnert and M.Abel. Numerical differentiation of experimental data: local versus global methods. <em>Computer Physics Communications</em>, 177:764–774, 2007.", gold);

    }
  }
}