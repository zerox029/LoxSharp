using System;

namespace Lox {
  class Lox {
    static Boolean hadError = false;

    static void Main(string[] args) {
      if (args.Length > 1) {
        Console.WriteLine("Usage: loxsharp [script]");
        System.Environment.Exit(1);
      } else if (args.Length == 1) {
        RunFile(args[0]);
      } else {
        RunPrompt();
      }
    }

    private static void RunFile(string path) {
      byte[] bytes = File.ReadAllBytes(path);
      Run(new string(bytes));

      if (hadError) 
        System.Environment.Exit(1);
    }
    
    private static void RunPrompt() {
      ///TODO
    }

    private static void Run(string source) {
      Scanner scanner = new Scanner();
      List<Token> tokens = scanner.ScanTokens();

      foreach (Token token in tokens) {
        Console.WriteLine(token);
      }
    }

    public static void Error(int line, string message) {
      Report(line, "", message);
    }

    private static void Report(int line, string where, string message) {
      Console.Error("[line " + line + "] Error" + where + ": " + message);
      hadError = true;
    }
  }
}
