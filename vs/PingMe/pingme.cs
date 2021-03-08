using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Console.WriteLine("found: " + process.StandardOutput);

        var pingproc = new Process();
        try
        {
          pingproc.StartInfo.FileName = process.StandardOutput.ToString();
          pingproc.StartInfo.Arguments = "-c 2 localhost";
          pingproc.StartInfo.CreateNoWindow = false;
          pingproc.StartInfo.UseShellExecute = false;
          pingproc.StartInfo.RedirectStandardOutput = true;

          Console.WriteLine("pingproc stdout: " + pingproc.StandardOutput);
          Console.WriteLine("pingproc stderr: " + pingproc.StandardError);

        } catch(Exception e) {
          Console.WriteLine("error on pingproc: " + e);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine($"EXCEPTION-1: {e}");
      }

      return false;
    }
  }
}
