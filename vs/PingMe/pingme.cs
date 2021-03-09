using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace PingMe
{
  public class pingme
  {
    private string version = "v0.4-2021";
    private int timeBetweenPings = 1000;

    private static string hostToPing = "";
    private bool logToFile = false;
    private static string logfile = "";
    private bool quietOutput = false;

    public pingme(string[] args)
    {
      /* no host/ip */
      if (args == null || args.Length == 0)
      {
        Console.WriteLine("No arguments specified!");
        printHelp();
        Environment.Exit(1);
      }

      /* parsing arguments */
      foreach (string a in args)
      {
        if (a.Contains("--help") || a.Contains("-h"))
        {
          printHelp();
          Environment.Exit(0);
        }

        if (a.Contains("-l"))
        {
          var i1 = Array.FindIndex(args, row => row.Contains("-l"));
          if (i1 >= 0)
          {
            if ((args.Length - 1) >= (i1 + 1))
            {
              logfile = args[i1 + 1];
              if (logfile.Length > 0)
              {
                logToFile = true;
              }
            }
            else
            {
              Console.WriteLine("YOU DIDN'T SPECIFY A LOGFILE!");
              Environment.Exit(1);
            }
          }
        }

        if (a.Contains("-i") || hostToPing.Length > 0)
        {
          var i1 = Array.FindIndex(args, row => row.Contains("-i"));
          if (i1 >= 0)
          {
            if ((args.Length - 1) >= (i1 + 1))
            {
              hostToPing = args[i1 + 1];
            }
            else
            {
              Console.WriteLine("YOU DIDN'T SPECIFY A HOST OR IP ADDRESS!");
              Environment.Exit(2);
            }
          }
        }

        if (a.Contains("-q"))
        {
          quietOutput = true;
        }
      }

      /* hostToPing is mandatory to run */
      if (hostToPing.Length <= 0)
      {
        Console.WriteLine("Please specify dns hostname or ip address!");
        Environment.Exit(7);
      }

      /* no logfile and quiet output are meant for this script */
      if (!logToFile && !quietOutput)
      {
        Console.WriteLine("No logfile and quiet output makes no sense.. ;)");
        Environment.Exit(3);
      }

      sendToOutput($"program version: {version}", true);
      sendToOutput($"running at: " + DateTime.Now, true);

      /* detect host/ip */
      if (Regex.IsMatch(hostToPing, @"^[a-zA-Z0-9.-]+$"))
      {
        /* verify ip address */
        if (Regex.IsMatch(hostToPing, @"^[0-9.]+$"))
        {
          IPAddress ip;
          IPAddress.TryParse(hostToPing, out ip);
          if (ip == null)
          {
            sendToOutput($"Wrong ip address detected! ({hostToPing})", true);
            Environment.Exit(4);
          }
          hostToPing = ip.ToString();
        }
        else
        {
          /* no ip detected - it must be a dns name */
          try
          {
            IPHostEntry host = Dns.GetHostEntry(hostToPing);
            hostToPing = host.HostName;
          }
          catch (Exception)
          {
            sendToOutput($"Host {hostToPing} cannot be resolved...", true);
            Environment.Exit(5);
          }
        }
      }
      else
      {
        sendToOutput("Wrong input detected! Are you kidding me?!", true);
        Environment.Exit(6);
      }

      sendToOutput($"host: {hostToPing}", true);

      /* os detection */
      var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
      var isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
      var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

      if (isLinux)
      {
        pingWithLinux(hostToPing);
      }
      else if (isOSX)
      {
        sendToOutput("OSX currently not implemented!", true);
      }
      else if (isWindows)
      {
        pingWithWindows(hostToPing);
      }
      else
      {
        sendToOutput("NO OPERATING SYSTEM DETECTED - WHAT ARE YOU USING?!", true);
        sendToOutput("If you are kind, please inform the developer!", true);
      }
    }

    private void pingWithLinux(string host)
    {
      bool isPingable = true;

      sendToOutput("initial ALIVE - " + DateTime.Now, true);
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
              sendToOutput($"\nHost {host} is now ALIVE - " + DateTime.Now, true);
              isPingable = true;
            }
            sendToOutput(".", false);
          }
          else
          {
            if (isPingable)
            {
              sendToOutput($"\nHost {host} is now DEAD - " + DateTime.Now, true);
              isPingable = false;
            }
            sendToOutput(".", false);
          }

          Thread.Sleep(timeBetweenPings);
        }
        catch (Exception)
        {
          sendToOutput("Problem finding program ping! Please inform the developer!", true);
        }
      }
    }

    private void pingWithWindows(string host)
    {
      bool isPingable = true;

      sendToOutput("initial ALIVE - " + DateTime.Now, true);
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
              sendToOutput($"\nHost {host} is now ALIVE - " + DateTime.Now, true);
              isPingable = true;
            }
            sendToOutput(".", false);
          }
          else
          {
            if (isPingable)
            {
              sendToOutput($"\nHost {host} is now DEAD - " + DateTime.Now, true);
              isPingable = false;
            }
            sendToOutput(".", false);
          }

          Thread.Sleep(timeBetweenPings);
        }
        catch (Exception e)
        {
          sendToOutput($"Problem finding program ping! Please inform the developer! (error: {e})", true);
        }
      }
    }

    private void sendToOutput(string message, bool newLine)
    {
      try
      {
        if (logToFile)
        {
          using (StreamWriter w = File.AppendText(logfile))
          {
            if (newLine)
            {
              w.WriteLine(message);
            }
            else
            {
              w.Write(message);
            }
          }
        }
      }
      catch (UnauthorizedAccessException)
      {
        Console.WriteLine($"Writing not allowed on {logfile}");
      }
      catch (Exception)
      {
        Console.WriteLine($"Error with logfile: {logfile}");
      }

      if (!quietOutput)
      {
        if (newLine)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine(message);
          Console.ResetColor();
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.Blue;
          Console.Write(message);
          Console.ResetColor();
        }
      }
    }

    private void printHelp()
    {
      Console.WriteLine("Usage: pingMe [args] -i host [args] ");
      Console.WriteLine("args can be: ");
      Console.WriteLine(" -h: help  - print this page");
      Console.WriteLine(" -i: input - dns hostname or ip address");
      Console.WriteLine("             this argument is mandatory!");
      Console.WriteLine(" -l: log   - write output to logfile and console");
      Console.WriteLine(" -q: quiet - no output on console except errors");
      Console.WriteLine("------- still under development! --------");
      Console.WriteLine(" -m: mail  - send mail to that address when status changes");
      Console.WriteLine("             currently only one mail address is possible.");
    }
  }
}
