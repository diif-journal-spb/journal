using System;
using System.Collections;

namespace EugenePetrenko.JournalGenerator
{
  public sealed class CommandLineParser
  {
    private Hashtable myParameters = new Hashtable();

    public CommandLineParser(string[] commandLine)
    {
      for (int i = 0; i < commandLine.Length; i++)
      {
        string param = commandLine[i];
        if (!param.StartsWith("/")) throw new CommandLineParseException("Unexpected key. All keys should starts from '/'");

        string key = param.Substring(1);

        String[] data = key.Split('=');

        if (data.Length > 2) throw new CommandLineParseException("Unexpected key style. Was :" + commandLine[i] + " but expected /key[=value]");

        if (data.Length == 1)
        {
          myParameters[data[0]] = "";
        }
        else
        {
          myParameters[data[0]] = data[1];
        }
      }
    }

    public string GetValue(string key)
    {
      return myParameters[key] as string;
    }

    public bool HasKey(string key)
    {
      return myParameters.ContainsKey(key);
    }
  }
}