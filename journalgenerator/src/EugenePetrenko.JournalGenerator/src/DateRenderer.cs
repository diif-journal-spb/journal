using System;
using Antlr.StringTemplate;

namespace EugenePetrenko.JournalGenerator
{
  public class DateRenderer : IAttributeRenderer
  {
    public String ToString(Object o)
    {
      return ((DateTime)o).ToString("yyyy.MM.dd");
    }
  }
}
