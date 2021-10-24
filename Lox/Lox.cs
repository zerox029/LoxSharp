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

      /*Expr expression = new Expr.Binary(
      new Expr.Unary(
        new Token(TokenType.MINUS, "-", null, 1),
        new Expr.Literal(123)),
      new Token(TokenType.STAR, "*", null, 1),
      new Expr.Grouping(
        new Expr.Literal(45.67)));
      
      Console.WriteLine(new AstPrinter().Print(expression));*/
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

      foreach (Token token in tokens) {
        Console.WriteLine(token);
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
