using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace PingMe
{
  public class pingme
  {
    public pingme()
    {
      // os detection
      var isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
      var isOSX = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
      var isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

      if (isLinux)
      {
        Console.WriteLine("LINUX FOUND");
      }
      else if (isOSX)
      {
        Console.WriteLine("OSX currently not implemented!");
      }
      else if (isWindows)
      {
        Console.WriteLine("WINDOWS FOUND");
      }
      else
      {
        Console.WriteLine("NO OPERATING SYSTEM DETECTED");
      }
    }


    private void pingWithLinux()
    {
      Console.WriteLine("PING WITH LINUX");
      var process = new Process();
      try
      {
        process.StartInfo.FileName = "ping";
        process.StartInfo.Arguments = "-c 2 localhost";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
       
        String s = process.StandardOutput.ReadToEnd();
        Console.WriteLine($"s: {s}");

      }
      catch (Exception e)
      {
        Console.WriteLine($"EXCEPTION-11: {e}");
      }

    }

    private void pingWithWindows()
    {
      Console.WriteLine("PING WITH WINDOWS");
      var process = new Process();
      try
      {
        process.StartInfo.FileName = "ping";
        process.StartInfo.Arguments = "-n 2 localhost";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();

        String s = process.StandardOutput.ReadToEnd();
        Console.WriteLine($"s: {s}");

      }
      catch (Exception e)
      {
        Console.WriteLine($"EXCEPTION-11: {e}");
      }

    }
  }
}
