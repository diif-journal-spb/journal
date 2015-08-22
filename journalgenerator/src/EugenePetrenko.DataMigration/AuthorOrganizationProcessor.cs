using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace EugenePetrenko.DataMigration
{
  public static class AuthorOrganizationProcessor
  {

    private class Hints
    {
      public string OrgId { get; private set; }
      
      public IEnumerable<string> Names { get; private set; }

      public Hints(string orgId, IEnumerable<string> names)
      {
        OrgId = orgId;
        Names = names;
      }

      public bool Matches(IEnumerable<string> addresses)
      {
        foreach (string address in addresses.Apply(ToMatch))
        {
          foreach (string orgName in Names.Apply(ToMatch))
          {
            if (address.Contains(orgName))
            {
              return true;
            }
          }
        }
        return false;
      }

      public override string ToString()
      {
        return "{Id = " + OrgId + ", Names = {" + string.Join(", ", Names) + "} }";
      }
    }

    private static IEnumerable<string> ToMatch(IEnumerable<string> input)
    {
      return input.Select(x => x.ToLower()).Apply(Normalize).Distinct();
    }

    private static IEnumerable<string> Normalize(IEnumerable<string> s)
    {
      return s
        .Where(x => x != null)
        .Select(x => Regex.Replace(x, @"\s+", " ").Trim())
        .Where(x=>x.Length > 0)
        .ToArray()
        ;
    }

    public static void Process(ErrorsCount ec, string dataDir)
    {
//      Debugger.Launch();

      var helpers = new List<Hints>();
      Util.ProcessFiles(ec, dataDir, "*.orgs", file => Util.UpdateXmlDocument(ec, file, root =>
      {
        if (root.Name != "orgs") throw new Exception(String.Format("Incorrect root element name {0} in {1}", root.Name, file));

        foreach (XmlElement orgElement in root.SelectNodes("org-xml"))
        {
          var id = orgElement.GetAttribute("id");
          if (id == "") continue;

          var names = orgElement.SelectNodes(".//name/text()|.//hint/text()")
            .Cast<XmlNode>()
            .Select(x => x.Value)
            .Apply(Normalize)
            .ToArray();

            
          if (!names.Any()) continue;
          helpers.Add(new Hints(id, names));
        }
      }));

      var allAuthorAddresses = new List<string>();
      Util.ProcessFiles(ec, dataDir, "*.authors", file => Util.UpdateXmlDocument(ec, file, element =>
      {
        if (element.Name != "authors-xml") throw new Exception(String.Format("Incorrect root element name {0} in {1}", element.Name, file));

        foreach (XmlElement author in element.SelectNodes("author"))
        {
          if (author.HasAttribute("org")) continue;

          var authorId = author.GetAttribute("id");

          var allAddresses = author.SelectNodes(".//Address/text()")
            .Cast<XmlNode>()
            .Select(x => x.Value)
            .Apply(Normalize)
            .ToArray();

          allAuthorAddresses.AddRange(allAddresses);

          if (!allAddresses.Any()) {
            ec.Error("Author {0} has no Addresses", authorId);
            continue;
          }

          var matches = helpers
            .Where(h => h.Matches(allAddresses))
            .ToList();

          if (matches.Count != 1) {
            ec.Error("Author {0} was not matched to orgs", authorId);
            continue;
          }
          
          var orgId = matches.Single().OrgId;
          author.SetAttribute("org", orgId);
          Console.Out.WriteLine("Author {0} was matched to {1}", authorId, orgId);          
        }
      }));

      var loggableAddresses = allAuthorAddresses.Distinct().Take(5);

      Console.Out.WriteLine("Possible addresses to include:");
      foreach (var loggableAddress in loggableAddresses)
      {
        Console.Out.WriteLine();
        Console.Out.WriteLine(loggableAddress);
      }
    }
  }
}
