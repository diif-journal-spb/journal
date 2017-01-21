namespace EugenePetrenko.JournalGenerator
{
  public sealed class Pair<TA,TB>
  {
    public readonly TA A;
    public readonly TB B;

    public Pair(TA a, TB b)
    {
      A = a;
      B = b;
    }
  }
}
