using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace PingMe
{
  public class pingme
  {
    private string version = "v0.3-2021";
    private int timeBetweenPings = 1000;
    private string hostToPing = "";

    public pingme(string[] args)
    {
      Console.WriteLine($"pingme version: {version}");

      // no host/ip 
      if (args == null || args.Length == 0)
      {
        Console.WriteLine("No hosts or ip specified!");
        Environment.Exit(1);
      }
      else
      {
        foreach (string a in args)
        {
          if (a.Contains("--help") || a.Contains("-h"))
          {
            printHelp();
            Environment.Exit(0);
          }

          if (a.Contains("-l") || a.Contains("--log"))
          {
            Console.WriteLine("-l / --log detected!");
            Console.WriteLine("jo und jetzt?");
          }
        }
      }


      // detect host/ip
      if (Regex.IsMatch(args[0], @"^[a-zA-Z0-9.-]+$"))
      {
        // verify ip address
        if (Regex.IsMatch(args[0], @"^[0-9.]+$"))
        {
          IPAddress ip;
          IPAddress.TryParse(args[0], out ip);
          if (ip == null)
          {
            Console.WriteLine($"Wrong ip address detected! ({args[0]})");
            Environment.Exit(1);
          }
          hostToPing = ip.ToString();
        }
        else
        {
          // no ip detected - it must be a dns name
          IPHostEntry host = null;

          try
          {
            host = Dns.GetHostEntry(args[0]);
            hostToPing = host.HostName;
          }
          catch (Exception)
          {
            Console.WriteLine($"Host {args[0]} cannot be resolved....");
            Environment.Exit(1);
          }
        }
      }
      else
      {
        Console.WriteLine("Wrong input detected! Are you kidding me?!");
        Environment.Exit(2);
      }

      Console.WriteLine($"host: {hostToPing}");

      // os detection
      var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
      var isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
      var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

      if (isLinux)
      {
        pingWithLinux(hostToPing);
      }
      else if (isOSX)
      {
        Console.WriteLine("OSX currently not implemented!");
      }
      else if (isWindows)
      {
        pingWithWindows(hostToPing);
      }
      else
      {
        Console.WriteLine("NO OPERATING SYSTEM DETECTED");
      }
    }

    private void pingWithLinux(string host)
    {
      bool isPingable = true;
      Console.WriteLine("initial ALIVE - " + DateTime.Now);
      while (true)
      {
        var process = new Process();
        try
        {
          process.StartInfo.FileName = "ping";
          process.StartInfo.Arguments = $"-c 1 -W 1 {host}";
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.Start();

          String s = process.StandardOutput.ReadToEnd();

          if (s.Contains(" 0% packet loss"))
          {
            if (!isPingable)
            {
              Console.WriteLine($"\n\033[32mHost {host} is now ALIVE\\e[0m - " + DateTime.Now);
              isPingable = true;
            }
            Console.Write(".");
          }
          else
          {
            if (isPingable)
            {
              Console.WriteLine($"\nHost {host} is now DEAD - " + DateTime.Now);
              isPingable = false;
            }
            Console.Write(".");
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
      bool isPingable = true;
      Console.WriteLine("initial ALIVE - " + DateTime.Now);
      while (true)
      {
        var process = new Process();
        try
        {
          process.StartInfo.FileName = "ping";
          process.StartInfo.Arguments = $"-n 1 -w 1000 {host}";
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.RedirectStandardOutput = true;
          process.StartInfo.RedirectStandardError = true;
          process.Start();

          String s = process.StandardOutput.ReadToEnd();

          if (s.Contains("(0% loss)"))
          {
            if (!isPingable)
            {
              Console.WriteLine($"\nHost {host} is now ALIVE - " + DateTime.Now);
              isPingable = true;
            }
            Console.Write(".");
          }
          else
          {
            if (isPingable)
            {
              Console.WriteLine($"\nHost {host} is now DEAD - " + DateTime.Now);
              isPingable = false;
            }
            Console.Write(".");
          }

          Thread.Sleep(timeBetweenPings);

        }
        catch (Exception e)
        {
          Console.WriteLine($"EXCEPTION-11: {e}");
        }
      }



    }

    private void printHelp()
    {
      Console.WriteLine("Usage: pingme {host} [args] ");
      Console.WriteLine("args can be: ");
      Console.WriteLine(" -h / --help: print this page");
      Console.WriteLine("------- still under development! --------");
      Console.WriteLine(" -l / --log: write output to logfile and console");
      Console.WriteLine(" -L / --logOnly: write output to logfile only");
      Console.WriteLine("                 does not output anything to console");
      Console.WriteLine(" -m / --mail: send mail to that address when status changes");
      Console.WriteLine("              currently only one mail address is possible.");
    }

    //private bool checkResolvablenessLinux(string host)
    //{
    //  bool retValue = false;
    //  var process = new Process();
    //  try
    //  {
    //    process.StartInfo.FileName = "ping";
    //    process.StartInfo.Arguments = $"-c 1 {host}";
    //    process.StartInfo.CreateNoWindow = true;
    //    process.StartInfo.UseShellExecute = false;
    //    process.StartInfo.RedirectStandardOutput = true;
    //    process.Start();

    //    String s = process.StandardOutput.ReadToEnd();

    //    if (s.Contains(" 0% packet loss"))
    //    {
    //      retValue = true;
    //    }
    //    else
    //    {
    //      retValue = false;
    //    }

    //  }
    //  catch (Exception e)
    //  {
    //    Console.WriteLine($"EXCEPTION-12: {e}");
    //    retValue = false;
    //  }
    //  return retValue;
    //}

    //private bool checkResolvablenessWindows(string host)
    //{
    //  bool retValue = false;
    //  var process = new Process();
    //  try
    //  {
    //    process.StartInfo.FileName = "ping";
    //    process.StartInfo.Arguments = $"-n 1 {host}";
    //    process.StartInfo.CreateNoWindow = true;
    //    process.StartInfo.UseShellExecute = false;
    //    process.StartInfo.RedirectStandardOutput = true;
    //    process.Start();

    //    String s = process.StandardOutput.ReadToEnd();

    //    if (s.Contains("(0% loss)"))
    //    {
    //      retValue = true;
    //    }
    //    else
    //    {
    //      retValue = false;
    //    }

    //  }
    //  catch (Exception e)
    //  {
    //    Console.WriteLine($"EXCEPTION-12: {e}");
    //    retValue = false;
    //  }
    //  return retValue;
    //}

  }
}
