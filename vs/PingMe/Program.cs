using System;

namespace PingMe
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      pingme pm = new pingme(args);
      Console.WriteLine("Goodbye World!");

    }
  }
}
