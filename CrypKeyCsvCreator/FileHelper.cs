using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypKeyCsvCreator
{
  public class FileHelper
  {
    public List<CrypKeyResult> ReadResults(string fileName)
    {
      IEnumerable<string> allLines = File.ReadLines(fileName);
      bool isInChunk = false;
      List<CrypKeyResult> crypKeyResults = new List<CrypKeyResult>();
      List<string> chunk = new List<string>();

      foreach (string allLine in allLines)
      {
        if (ChunkStarts(allLine))
        {
          chunk = new List<string>();
          isInChunk = true;
        }

        if (ChunkEnds(allLine))
        {
          isInChunk = false;
          crypKeyResults.Add(new CrypKeyResult(chunk));
        }

        if (isInChunk)
        {
          chunk.Add(allLine);
        }
      }

      return crypKeyResults;
    }

    public void AppendToFile(List<CrypKeyResult> results, string fileName)
    {
      if (!File.Exists(fileName))
      {
        CreateNewFile(fileName);
      }

      var lastDateTime = GetLastEntryTime(fileName);

      foreach (var crypKeyResult in results)
      {
        if (crypKeyResult.DateOfIssue > lastDateTime)
        {
          File.AppendAllText(fileName, CreateLine(crypKeyResult));
        }
      }
    }

    private bool ChunkStarts(string line)
    {
      return line.StartsWith("Date of Issue");
    }

    private bool ChunkEnds(string line)
    {
      return line.Equals("End");
    }

    private void CreateNewFile(string fileName)
    {
      string header =
        "Date of issue;Issued to;Productname;Level;License type;Number of copies;Restriction type;Days or runs;SiteCode;SiteKey;HD serial;Options\n";
      File.WriteAllText(fileName, header);
    }


    private string CreateLine(CrypKeyResult crypKeyResult)
    {
      var builder = new StringBuilder();
      string optionEntry = String.Empty;

      builder.Append($"{crypKeyResult.DateOfIssue};");
      builder.Append($"{crypKeyResult.IssuedTo};");
      builder.Append($"{crypKeyResult.ProductName};");
      builder.Append($"{crypKeyResult.Level};");
      builder.Append($"{crypKeyResult.LicenseType};");
      builder.Append($"{crypKeyResult.NumberOfCopies};");
      builder.Append($"{crypKeyResult.RestrictionType};");
      builder.Append($"{crypKeyResult.NumberOfDaysOrRuns};");
      builder.Append($"{crypKeyResult.SiteCode};");
      builder.Append($"{crypKeyResult.SiteKey};");
      builder.Append($"{crypKeyResult.HdSerial};");
      
      foreach (var option in crypKeyResult.Options)
      {
        optionEntry += $"{option}-";
      }

      builder.Append($"{optionEntry}\n");

      return builder.ToString();
    }

    private DateTime GetLastEntryTime(string fileName)
    {
      DateTime lastEntryDateTime = DateTime.MinValue;
      string[] lines = File.ReadAllLines(fileName);
      string lastLine = lines[lines.Length - 1];
      DateTime.TryParse(lastLine.Substring(0, lastLine.IndexOf(";")), out lastEntryDateTime);

      return lastEntryDateTime;
    }
  }
}
