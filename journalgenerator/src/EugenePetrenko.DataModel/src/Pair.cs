namespace EugenePetrenko.DataModel
{
  public static class Pair
  {
    public static Pair<A,B> Create<A,B>(A a, B b)
    {
      return new Pair<A, B>(a, b);
    }
  }

  public class Pair<TA,TB>
  {
    private readonly TA myA;
    private readonly TB myB;

    public Pair(TA a, TB b)
    {
      myA = a;
      myB = b;
    }

    public TA A => myA;

    public TB B => myB;
  }
}
