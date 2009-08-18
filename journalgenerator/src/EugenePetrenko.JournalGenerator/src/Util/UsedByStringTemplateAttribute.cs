using System;
using JetBrains.Annotations;

namespace EugenePetrenko.JournalGenerator.Util
{
  [AttributeUsage(AttributeTargets.Property)]
  [MeansImplicitUse]
  public class UsedByStringTemplateAttribute : Attribute
  {    
  }
}


