using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NRegFreeCom;

namespace TestRegFileParser
{
  class Program
  {
    static string outputPath = Path.Combine(Environment.CurrentDirectory,"regfile.txt");

    static void Main(string[] args)
    {
      Console.WriteLine("Registry File Parser");

      if (args.Length == 0)
      {
        Console.WriteLine("Missing paramter(s).");
        Console.WriteLine("Syntax:");
        Console.WriteLine("RegFileParser <reg file path> [<output file path>]");
        Console.WriteLine();
        
        //Wait for ENTER key to close program
        Console.WriteLine("Press 'ENTER' to exit program.");
        Console.ReadLine();

        return;
      }

      if (!File.Exists(args[0]))
      {
        Console.WriteLine("File '{0}' not found.", args[0]);
        
        //Wait for ENTER key to close program
        Console.WriteLine("Press 'ENTER' to exit program.");
        Console.ReadLine();
        return;
      }

      if (args.Length == 0)
      {
        outputPath = args[1];
      }

      try
      {
        var file = new FileStream(args[0],FileMode.Open);
        var regfile = new RegFileReader(file);

        Console.WriteLine("Reg file has been imported.");

        //For proofing purpose generate new txt file with imported content
        Int32 count = 0;
        var sb = new StringBuilder();
        foreach (KeyValuePair<String, Dictionary<String, RegValueObject>> entry in regfile.RegValues)
        {
          sb.AppendLine(String.Format(@"[{0}]", entry.Key));
          foreach (KeyValuePair<String, RegValueObject> item in entry.Value)
          {
            if (String.IsNullOrEmpty(item.Value.Entry))
              sb.Append("@=");
            else
              sb.AppendFormat("\"{0}\"=", item.Value.Entry);

            if (item.Value.Type == "REG_SZ")
              sb.AppendLine(String.Format("\"{0}\"", item.Value.Value));
            else
              sb.AppendLine(String.Format("[{0}] {1}", item.Value.Type, item.Value.Value));
            count++;
          }
          sb.AppendLine();
        }
        File.WriteAllText(outputPath, sb.ToString());
        Console.WriteLine("Content file generated as '{0}'", outputPath);

        Console.WriteLine("Reg file contains {0} keys and {1} values.", regfile.RegValues.Count, count);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Exception thrown\n{0}", ex);
      }

      //Wait for ENTER key to close program
      Console.WriteLine("Press 'ENTER' to exit program.");
      Console.ReadLine();

    }
  }
}
