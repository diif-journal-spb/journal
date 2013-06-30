using System;
using NUnit.Framework;

namespace EugenePetrenko.NumberEditor
{
  [TestFixture]
  public class ReferencesTest
  {
   
    [Test]
    public void parse()
    {
      var text = @"1.	B. C. Dhage, Multi-valued condensing random operators and functional random integral 
        inclusions, Opuscula Mathematica,Vol.31, No.1, 2011.
2.	B. C. Dhage and S. M. Kang,Functional random boundary value problems of 
        second order differential equations, Pan American Mathematical Journal 15 (4), 2005, 59-72.
3.	B. C. Dhage, S. K. Ntouyas, D. S. Palimkar, On boundary value problems of second 
        order convex and non-convex differential inclusions, Fixed point Theory, Vol. 9 No. 1,2008,89-104.
4.	Hu and N. S. Papageorgiou, Hand Book of Multi-valued Analysis Vol.-I, 
        Theory Kluwer Academic Publisher, Dordrechet, Boston, London, 1997. 
5.	S. Itoh, Random fixed point theorems with applications to random differential 
        equations in Banach spaces, J. Math. Anal. Appl. 67, 1979, 261-273.
6.	K. Kuratouski,C. Ryll-Nardzeuskii, A general theorem on selectors, 
        Bull. Acad. Pol. Sci. Ser. Math. Sci. Astron.Phys.13, 1965, 397-403.
7.	A. Lasota and Z. Opial, An application of Kakutani-Ky Fan theorem in the theory 
        of ordinary differential equations, Bull. Acad. Pol. Sci. Ser. Sci. Math. Astronom. Phys. 13, 
        1965, 781-786.
8.	N. S. Papageorgiou, Random Fixed points and random differential inclusions, 
        Intern. J. Math. and Math. Sci. 11(03), 1988, 551-560.
9.	D.S. Palimkar, Existence Theory of Random Differential Equations , 
        International Journal of Scientific and Research Publications, Vol. 2, No. 7, 2012,1-6.
10.	D.S. Palimkar, Boundary Value Problem of Second Order Differential Inclusion , 
        International Journal of Mathematics Research, Vol. 4, No. 5, 2012,527-533.
11.	D.S. Palimkar, Existence Theory for Second Order Random Differential Inclusion, 
        International Journal of Advances in Engineering, Science and Technology, Vol. 2, 
        No. 3, 2012, 261-266.
12.	D.S. Palimkar, Existence Theory of Second Order Random Differential	Equations, 
        Global Journal of Mathematics and Mathematical Sciences, Vol. 2, No. 1, 2012,7-15.
13.	D.S. Palimkar, Second Order Random Differential Inclusion, 
        Variorum Multi-Disciplinary e-Research Journal, Vol. 1, No. 3, 2011,1-16.
14.	D.S. Palimkar, Some Studies in Random Differential Inclusions, 
        Lambert Publication, Germany, ";


      var references = ReferencesParser.ParseReferences(text);

      foreach (var re in references)
      {
        Console.Out.WriteLine("re = {0}", re);
      }
    }
 
  }
}