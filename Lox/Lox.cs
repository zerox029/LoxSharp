using System;
using System.IO;
using System.Collections.Generic;

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
      Run(new string(System.Text.Encoding.UTF8.GetString(bytes)));

      if (hadError)
        System.Environment.Exit(1);
    }

    private static void RunPrompt() {
      ///TODO
    }

    private static void Run(string source) {
      Scanner scanner = new Scanner(source);
      List<Token> tokens = scanner.ScanTokens();

      Parser parser = new Parser(tokens);
      Expr expr = parser.Parse();

      if (hadError) return;

      Console.WriteLine(new AstPrinter().Print(expr));

      /*
      foreach (Token token in tokens) {
        Console.WriteLine(token);
      }*/
    }

    public static void Error(Token token, string message) {
      if (token.Type == TokenType.EOF) {
        Report(token.Line, " at end", message);
      } else {
        Report(token.Line, " at '" + token.Lexeme + "'", message);
      }
    }

    public static void Error(int line, string message) {
      Report(line, "", message);
    }

    private static void Report(int line, string where, string message) {
      Console.WriteLine("!! [line " + line + "] Error" + where + ": " + message + " !!");
      hadError = true;
    }
  }
}
