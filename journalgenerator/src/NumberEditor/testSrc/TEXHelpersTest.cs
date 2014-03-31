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

      Assert.AreEqual("K.Ahnert and M.Abel. Numerical differentiation of experimental data: local versus global methods. <em>Computer Physics Communications</em>, 177:764-774, 2007.", gold);

    }

    [Test]
    public void test_replaces_uppercase()
    {
      var input = @"\bibitem{Demet07}
M.A. Demetriou.
\newblock Process estimation and moving source detection in 2-\uppercase{D}
  diffusion processes by scheduling of sensor networks.
\newblock In {\em Proceedings of the American Control Conference}, New York,
  NY, July 2007.
";

      var gold = TEXHelpers.FixTexIntoHTML(input);

      Console.Out.WriteLine(gold);

      Assert.AreEqual("M.A. Demetriou. Process estimation and moving source detection in 2-D diffusion processes by scheduling of sensor networks. In <em>Proceedings of the American Control Conference</em>, New York, NY, July 2007.", gold);

    }

    [Test]
    public void test_replaces_tex_symbols_1()
    {
      var input = @"\bibitem{MeHeAs08}
A.~Mesquita, J.~Hespanha, and K.~\r{A}str\""{o}m.
\newblock Optimotaxis: a stochastic multi-agent optimization procedure with
  point measurements.
\newblock In M.~Egersted and B.~Mishra, editors, {\em Hybrid Systems:
  Computation and Control}, volume 4981, pages 358–--371. Springer-Verlag,
  Berlin, 2008.
";

      var gold = TEXHelpers.FixTexIntoHTML(input);

      Console.Out.WriteLine(gold);

      Assert.AreEqual("A.Mesquita, J.Hespanha, and K.Åström. Optimotaxis: a stochastic multi-agent optimization procedure with point measurements. In M.Egersted and B.Mishra, editors, <em>Hybrid Systems: Computation and Control</em>, volume 4981, pages 358-371. Springer-Verlag, Berlin, 2008.", gold);
    }
    
    [Test]
    public void test_replaces_tex_symbols_2()
    {
      var input = @"\bibitem{TzZe07}
P.~Tzanos and M.~\u{Z}efran.
\newblock Locating a circular biochemical source: \uppercase{M}odeling and
  control.
\newblock In {\em Proceedings of the 2007 IEEE Int. Conf. on Robotics and
  Automation}, pages 523--528, Rome, Italy, 2007.
";

      var gold = TEXHelpers.FixTexIntoHTML(input);

      Console.Out.WriteLine(gold);

      Assert.AreEqual("P.Tzanos and M.Žefran. Locating a circular biochemical source: Modeling and control. In <em>Proceedings of the 2007 IEEE Int. Conf. on Robotics and Automation</em>, pages 523-528, Rome, Italy, 2007.", gold);

    }

    [Test]
    public void test_replaces_tex_symbols_3()
    {
      var input = @"\bibitem{VaKh08}
L.~K. Vasiljevic and H.~K. Khalil.
\newblock Error bounds in differentiation of noisy signals by high-gain
  observers.
\newblock {\em Systems \& Control Letters}, 57:856--862, 2008.
";

      var gold = TEXHelpers.FixTexIntoHTML(input);

      Console.Out.WriteLine(gold);

      Assert.AreEqual("L.K. Vasiljevic and H.K. Khalil. Error bounds in differentiation of noisy signals by high-gain observers. <em>Systems & Control Letters</em>, 57:856-862, 2008.", gold);

    }

    [Test]
    public void test_replaces_tex_symbols_4()
    {
      var input = @"\bibitem{Vin00}
R.~B. Vinter.
\newblock {\em Optimal Control}.
\newblock Birkh\""{a}uzer, Boston, 2000.
";
      var gold = TEXHelpers.FixTexIntoHTML(input);

      Console.Out.WriteLine(gold);

      Assert.AreEqual("R.B. Vinter. <em>Optimal Control</em>. Birkhäuzer, Boston, 2000.", gold);

    }

    [Test]
    public void test_replaces_tex_symbols_5()
    {
      var input = @"\bibitem{OgFiLe04}
P.~\""{O}gren, E.~Fiorelli, and N.~E. Leonard.
\newblock Cooperative control of mobile sensor networks: Adaptive gradient
  climbing in a distributed environment.
\newblock {\em IEEE Trans. Autom. Control}, 49(8):1292--1301, 2004.
";
      var gold = TEXHelpers.FixTexIntoHTML(input);

      Console.Out.WriteLine(gold);

      Assert.AreEqual("P.Ögren, E.Fiorelli, and N.E. Leonard. Cooperative control of mobile sensor networks: Adaptive gradient climbing in a distributed environment. <em>IEEE Trans. Autom. Control</em>, 49(8):1292-1301, 2004.", gold);

    }
  }
}