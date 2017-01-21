using System;

namespace EugenePetrenko.JournalGenerator
{
  public class CommandLineParseException : Exception
  {
    public CommandLineParseException(string message)
      : base(message)
    {
    }
  }
}
