using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace EugenePetrenko.NumberEditor
{
  public interface Localizable
  {
    string Lang { get; set; }
  }

  [Serializable]
  public abstract class LocalizableHolder<T> where T : class, Localizable
  {
    protected abstract T NewT();

    [XmlIgnore]
    public T EN { get; set; }

    [XmlIgnore]
    public T RU { get; set; }

    public T GetEN()
    {
      if (EN != null) return EN;
      EN = NewT();
      EN.Lang = "EN";
      return EN;
    }

    public T GetRU()
    {
      if (RU != null) return RU;
      RU = NewT();
      RU.Lang = "RU";
      return RU;
    }

    public void InitLanguages(IEnumerable<string> langs)
    {
      foreach (var lang in langs)
      {
        if (lang == "EN") GetEN();
        if (lang == "RU") GetRU();
      }
    }
    
    [XmlIgnore]
    protected T[] ItemsIntenal
    {
      get { return new[] { EN, RU }.Where(x => x != null).ToArray(); }
      set
      {
        EN = null;
        RU = null;

        if (value != null)
        {
          foreach (var info in value)
          {
            if (info.Lang == "EN") EN = info;
            if (info.Lang == "RU") RU = info;
          }
        }
      }
    }
    
  }
}