using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PingMe
{
  public class pingme
  {
    public pingme()
    {
      if (FindProgram())
      {
        Console.WriteLine("TRUE");
      }
      else
      {
        Console.WriteLine("FALSE");
      }
    }

    private bool FindProgram()
    {
      var process = new Process();
      try
      {
        process.StartInfo.FileName = "which";
        process.StartInfo.Arguments = "ping";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        //Console.WriteLine("found1: " + process.StandardOutput);
        //Console.WriteLine("found2: " + process.StandardOutput.ReadToEnd());
        //StreamReader sr = new StreamReader(process.StandardOutput.ReadToEnd());
        String s = process.StandardOutput.ReadToEnd();
        Console.WriteLine($"s: {s}");

        var pingproc = new Process();
        try
        {
          pingproc.StartInfo.FileName = s;
          pingproc.StartInfo.Arguments = "-c 2 localhost";
          pingproc.StartInfo.CreateNoWindow = false;
          pingproc.StartInfo.UseShellExecute = false;
          pingproc.StartInfo.RedirectStandardOutput = true;
          pingproc.Start();
          //StreamReader sr2 = new StreamReader(pingproc.StandardOutput.ReadToEnd());
          String s2 = pingproc.StandardOutput.ReadToEnd();
          Console.WriteLine($"s2: {s2}");

        }
        catch (Exception e)
        {
          Console.WriteLine("error on pingproc: " + e);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine($"EXCEPTION-11: {e}");
      }

      return false;
    }
  }
}
