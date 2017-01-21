using System;
using System.Collections.Generic;
using System.IO;
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
      return input
        .Select(x => x.ToLower())
        .Select(x => x.Replace(",", " "))
        .Select(x => x.Replace(".", " "))
        .Apply(Normalize)
        .Distinct();
    }

    private static IEnumerable<string> Normalize(IEnumerable<string> s)
    {
      return s
        .Where(x => x != null)
        .Select(x => Regex.Replace(x, @"\s+", " ").Trim())
        .Where(x => x.Length > 0)
        .ToArray()
        ;
    }

    public static void Process(ErrorsCount ec, string dataDir)
    {
      Console.Out.WriteLine("=====");

      var helpers = new List<Hints>();
      Util.ProcessFiles(ec, dataDir, "*.orgs", file => Util.UpdateXmlDocument(ec, file, root =>
      {
        if (root.Name != "orgs")
          throw new Exception($"Incorrect root element name {root.Name} in {file}");

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

      var duplicateIds = helpers.Select(x=>x.OrgId).GroupBy(x=>x).Where(x=>x.Count() > 1).ToArray();
      if (duplicateIds.Any())
      {
        foreach (var id in duplicateIds)
        {
          ec.Error("Duplicate ordId: {0}", id.First());
        }
      }

      var allAuthorAddresses = new List<IEnumerable<string>>();
      var allAuthorIDs = new Dictionary<String, String>();
      Util.ProcessFiles(ec, dataDir, "*.authors", file => Util.UpdateXmlDocument(ec, file, element =>
      {
        if (element.Name != "authors-xml")
          throw new Exception($"Incorrect root element name {element.Name} in {file}");

        foreach (XmlElement author in element.SelectNodes("author"))
        {
          var authorId = author.GetAttribute("id");

          if (allAuthorIDs.ContainsKey(authorId))
          {
            ec.Error("Author ID {0} is used in {1} and {2}", authorId, file, allAuthorIDs[authorId]);
            allAuthorIDs[authorId] += ", " + file;
          }
          else
          {
            allAuthorIDs[authorId] = file;
          }

          if (author.HasAttribute("org")) {
            var orgId = author.GetAttribute("org");
            if (helpers.Count(h => h.OrgId == orgId) != 1) {
              ec.Error("Author {0} has incorrect org: {1}", authorId, orgId);
            }
            continue;
          }

          var allAddresses = author.SelectNodes(".//Address/text()")
            .Cast<XmlNode>()
            .Select(x => x.Value)
            .Apply(Normalize)
            .ToArray();

          if (!allAddresses.Any() && !author.GetAttribute("allow-no-address").Equals("true", StringComparison.InvariantCultureIgnoreCase))
          {
            ec.Error("Author {0} has no Addresses", authorId);
            continue;
          }

          var matches = helpers
            .Where(h => h.Matches(allAddresses))
            .ToList();

          if (matches.Count == 0)
          {
            ec.Error("Author {0} was not matched to orgs", authorId);
            allAuthorAddresses.Add(allAddresses.ToArray());
            continue;
          }

          if (matches.Count > 1)
          {
            ec.Error("!!! Author {0} was not matched to orgs: {1}", authorId, string.Join(", ", matches.Select(x=>x.OrgId).OrderBy(x=>x)));
            continue;
          }

          {
            var orgId = matches.Single().OrgId;
            author.SetAttribute("org", orgId);
            Console.Out.WriteLine("Author {0} was matched to {1}", authorId, orgId);
          }
        }
      }));

      var template = Path.Combine(dataDir, "orgs.template");
      if (File.Exists(template))
      {
        File.Delete(template);
      }

      if (!allAuthorAddresses.Any()) return;

      var loggableAddresses = allAuthorAddresses.Distinct().Take(5);

      Console.Out.WriteLine("Possible addresses to include:");
      foreach (var loggableAddress in loggableAddresses)
      {
        Console.Out.WriteLine();
        foreach (var addr in loggableAddress)
        {
          Console.Out.WriteLine(addr);          
        }
      }

      Util.UpdateOrCreateXmlDocument(ec, template, el =>
      {
        foreach (var address in allAuthorAddresses)
        {
          var orgRootElement = el.OwnerDocument.CreateElement("org-xml");
          orgRootElement.SetAttribute("id", "");

          foreach (var addr in address)
          {
            var orgLangElement = el.OwnerDocument.CreateElement("org");
            orgLangElement.SetAttribute("lang", Regex.Matches(addr, "[а-я]+", RegexOptions.IgnoreCase).Count > 0 ? "RU" : "EN");

            var nameElement = el.OwnerDocument.CreateElement("name");
            nameElement.AppendChild(el.OwnerDocument.CreateTextNode(addr));

            var addressElement = el.OwnerDocument.CreateElement("address");
            addressElement.AppendChild(el.OwnerDocument.CreateTextNode(addr));

            orgLangElement.AppendChild(nameElement);
            orgLangElement.AppendChild(addressElement);

            orgRootElement.AppendChild(orgLangElement);
          }

          el.AppendChild(orgRootElement);
        }
      }, doc => doc.AppendChild(doc.CreateElement("orgs")));
    }
  }
}
