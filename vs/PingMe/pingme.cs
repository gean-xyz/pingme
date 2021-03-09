using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace PingMe
{
  public class pingme
  {
    private int timeBetweenPings = 1000;
    private string hostToPing = "";

    public pingme(string[] args)
    {
      // no host/ip 
      if (args.Length == 0)
      {
        Console.WriteLine("No hosts specified!");
        Environment.Exit(1);
      }

      // detect input
      if (Regex.IsMatch(args[0], @"^[a-zA-Z0-9.-]+$"))
      {
        Console.WriteLine("good input detected!");
        hostToPing = args[0];
      }
      else
      {
        Console.WriteLine("false input detected!");
        Environment.Exit(2);
      }


      // os detection
      var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
      var isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
      var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

      if (isLinux)
      {
        if (checkResolvablenessLinux(hostToPing))
        {
          pingWithLinux(hostToPing);
        }
        else
        {
          Console.WriteLine($"Host {hostToPing} is not resolvable! Aborting..");
        }
      }
      else if (isOSX)
      {
        Console.WriteLine("OSX currently not implemented!");
      }
      else if (isWindows)
      {
        if (checkResolvablenessWindows(hostToPing))
        {
          pingWithWindows(hostToPing);
        }
        else
        {
          Console.WriteLine($"Host {hostToPing} is not resolvable! Aborting..");
        }
      }
      else
      {
        Console.WriteLine("NO OPERATING SYSTEM DETECTED");
      }
    }

    private bool checkResolvablenessLinux(string host)
    {
      bool retValue = false;
      var process = new Process();
      try
      {
        process.StartInfo.FileName = "ping";
        process.StartInfo.Arguments = $"-c 1 {host}";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();

        String s = process.StandardOutput.ReadToEnd();

        if (s.Contains(" 0% packet loss"))
        {
          retValue = true;
        }
        else
        {
          retValue = false;
        }

      }
      catch (Exception e)
      {
        Console.WriteLine($"EXCEPTION-12: {e}");
        retValue = false;
      }
      return retValue;
    }

    private bool checkResolvablenessWindows(string host)
    {
      bool retValue = false;
      var process = new Process();
      try
      {
        process.StartInfo.FileName = "ping";
        process.StartInfo.Arguments = $"-n 1 {host}";
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();

        String s = process.StandardOutput.ReadToEnd();

        if (s.Contains("(0% loss)"))
        {
          retValue = true;
        }
        else
        {
          retValue = false;
        }

      }
      catch (Exception e)
      {
        Console.WriteLine($"EXCEPTION-12: {e}");
        retValue = false;
      }
      return retValue;
    }

    private void pingWithLinux(string host)
    {
      bool isPingable = false;
      while (true)
      {
        var process = new Process();
        try
        {
          process.StartInfo.FileName = "ping";
          process.StartInfo.Arguments = $"-c 1 {host}";
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.Start();

          String s = process.StandardOutput.ReadToEnd();

          if (s.Contains(" 0% packet loss"))
          {
            if (isPingable)
            {
              Console.Write(".");
            }
            else
            {
              Console.WriteLine($"\nHost {host} is now ALIVE - " + DateTime.Now);
            }
            isPingable = true;
          }
          else
          {
            if (isPingable)
            {
              Console.WriteLine($"\nHost {host} is now DEAD - " + DateTime.Now);
            }
            else
            {
              Console.Write(".");
            }
            isPingable = false;
          }

          Thread.Sleep(timeBetweenPings);

        }
        catch (Exception e)
        {
          Console.WriteLine($"EXCEPTION-11: {e}");
        }
      }

    }

    private void pingWithWindows(string host)
    {
      bool isPingable = false;
      while (true)
      {
        var process = new Process();
        try
        {
          process.StartInfo.FileName = "ping";
          process.StartInfo.Arguments = $"-n 1 {host}";
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.RedirectStandardOutput = true;
          process.Start();

          String s = process.StandardOutput.ReadToEnd();

          if (s.Contains("(0% loss)"))
          {
            if (isPingable)
            {
              Console.Write(".");
            }
            else
            {
              Console.WriteLine($"Host {host} is now ALIVE - " + DateTime.Now);
            }
            isPingable = true;
          }
          else
          {
            if (isPingable)
            {
              Console.WriteLine($"Host {host} is now DEAD - " + DateTime.Now);
            }
            else
            {
              Console.Write(".");
            }
            isPingable = false;
          }

          Thread.Sleep(timeBetweenPings);

        }
        catch (Exception e)
        {
          Console.WriteLine($"EXCEPTION-11: {e}");
        }
      }



    }
  }
}
