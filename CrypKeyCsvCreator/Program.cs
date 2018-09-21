using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypKeyCsvCreator
{
  class Program
  {
    private static List<CrypKeyResult> results;
    static void Main(string[] args)
    {
      FileHelper helper = new FileHelper();

      results = helper.ReadResults(Properties.Settings.Default.CrypKeyLogFile);

      helper.AppendToFile(results, Properties.Settings.Default.CsvFileName);
    }
  }
}
