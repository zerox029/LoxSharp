using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tools {
  class GenerateAst {
    static void Main(string[] args) {
      if(args.Length != 1) {
        Console.WriteLine("Usage: generate_ast <output directory>");
        System.Environment.Exit(1);
      }

      string outputDir = args[0];
      string[] types = { 
        "Binary   : Expr left, Token op, Expr right",
        "Grouping : Expr expression",
        "Literal  : Object value",
        "Unary    : Token op, Expr right"
      };
      DefineAst(outputDir, "Expr", types.ToList());
    }

    private static void DefineAst(string outputDir, string baseName, List<string> types) {
      string path = $"{outputDir}/{baseName}.cs";
      List<string> lines = new List<string>();

      lines.Add("using System;");
      lines.Add("");
      lines.Add("namespace Lox {");
      lines.Add("abstract class " + baseName + "{");

      foreach(string type in types) {
        string className = type.Split(":")[0].Trim();
        string fields = type.Split(":")[1].Trim();
        DefineType(lines, baseName, className, fields);
      }

      lines.Add("}");
      lines.Add("}");

      File.WriteAllLines(path, lines);
    }

    private static void DefineType(List<string> lines, string baseName, string className, string fieldList) {
      lines.Add("class " + className + " : " + baseName + " {");

      // Fields
      string[] fields = fieldList.Split(", ");
      foreach(string field in fields) {
        lines.Add("readonly " + field + ";");
      }

      lines.Add("");

      // Constructor
      lines.Add(className + "(" + fieldList + ") {");

      // Storing the fields
      foreach(string field in fields) {
        string name = field.Split(" ")[1];
        lines.Add("this." + name + " = " + name + ";");
      }

      lines.Add("}");
      lines.Add("}");
    }
  }
}
