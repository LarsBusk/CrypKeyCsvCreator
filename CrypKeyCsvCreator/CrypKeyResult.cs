using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CrypKeyCsvCreator
{
  public class CrypKeyResult
  {
    #region public properties

    public DateTime DateOfIssue { get; private set; }

    public string IssuedTo { get; private set; }

    public string ProductName { get; private set; }

    public int Level { get; private set; }

    public string LicenseType { get; private set; }

    public int NumberOfCopies { get; private set; }

    public string RestrictionType { get; private set; }

    public int NumberOfDaysOrRuns { get; private set; }

    public string SiteCode { get; private set; }

    public string SiteKey { get; private set; }

    public string HdSerial { get; private set; }

    public List<string> Options { get; private set; }

#endregion

    private readonly List<string> chunk;

    public CrypKeyResult()
    {
      Options = new List<string>();
    }

    public CrypKeyResult(List<string> licenseChunk)
    {
      Options = new List<string>();
      chunk = licenseChunk;
      DateOfIssue = DateTime.Parse(FindInChunk("Date of Issue:"));
      IssuedTo = FindInChunk("Issued_To:");
      ProductName = FindInChunk("Product_name:");
      Level = int.Parse(FindInChunk("Level:"));
      LicenseType = FindInChunk("License_Type:");
      NumberOfCopies = int.Parse(FindInChunk("Number_Of_Copies:"));
      RestrictionType = FindInChunk("Restriction_Type:");
      NumberOfDaysOrRuns = int.Parse(FindInChunk("Number_Days_or_runs:"));
      SiteCode = FindInChunk("SiteCode:");
      SiteKey = FindInChunk("SiteKey:");
      HdSerial = FindInChunk("HDSerial:");

      foreach (string line in licenseChunk)
      {
        if (IsOption(line))
        {
          Options.Add(line);
        }
      }
    }

    
    private string FindInChunk(string identifier)
    {
      return chunk.Find(l => l.StartsWith(identifier)).Substring(identifier.Length);
    }

    private bool IsOption(string line)
    {
      if (line.IndexOf(":") > 0)
      {
        int number;
        return int.TryParse(line.Substring(0, line.IndexOf(":")), out number);
      }

      return false;
    }
  }
}
