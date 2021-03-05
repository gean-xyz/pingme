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
      }
      catch (Exception e)
      {
        Console.WriteLine($"EXCEPTION-1: {e}");
      }

      return false;
    }
  }
}
