namespace EugenePetrenko.JournalGenerator
{
  public sealed class Pair<TA,TB>
  {
    private readonly TA myA;
    private readonly TB myB;

    public Pair(TA a, TB b)
    {
      myA = a;
      myB = b;
    }

    public TA A
    {
      get { return myA; }
    }

    public TB B
    {
      get { return myB; }
    }
  }
}