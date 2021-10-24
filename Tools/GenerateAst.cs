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
      lines.Add($"public abstract class {baseName} {{");

      DefineVisitor(lines, baseName, types);

      foreach(string type in types) {
        lines.Add("");

        string className = type.Split(":")[0].Trim();
        string fields = type.Split(":")[1].Trim();
        DefineType(lines, baseName, className, fields);
      }

      lines.Add("");
      lines.Add($"public abstract T Accept<T>(I{baseName}Visitor<T> visitor);");

      lines.Add("}");
      lines.Add("}");

      File.WriteAllLines(path, lines);
    }

    private static void DefineVisitor(List<string> lines, string baseName, List<string> types) {
      lines.Add($"public interface I{baseName}Visitor<T> {{");

      foreach(string type in types) {
        string typeName = type.Split(":")[0].Trim();
        lines.Add($"T Visit{typeName}{baseName} ({baseName}.{typeName} {baseName.ToLower()});");
      }

      lines.Add("}");
    }

    private static void DefineType(List<string> lines, string baseName, string className, string fieldList) {
      lines.Add($"public class {className} : {baseName} {{");

      // Fields
      string[] fields = fieldList.Split(", ");
      foreach(string field in fields) {
        string type = field.Split(" ")[0];
        string name = field.Split(" ")[1];
        lines.Add($"public {type} {UppercaseFirst(name)} {{ get; }}");
      }

      lines.Add("");

      // Constructor
      lines.Add($"public {className} ({fieldList}) {{");

      // Storing the fields
      foreach(string field in fields) {
        string name = field.Split(" ")[1];
        lines.Add($"this.{UppercaseFirst(name)} = {name};");
      }

      lines.Add("}");

      lines.Add("");
      lines.Add($"public override T Accept<T>(I{baseName}Visitor<T> visitor) {{");
      lines.Add($"return visitor.Visit{className}{baseName} (this);");
      lines.Add("}");

      lines.Add("}");
    }

    private static string UppercaseFirst(string s)
    {
      // Check for empty string.
      if (string.IsNullOrEmpty(s))
      {
          return string.Empty;
      }

      // Return char and concat substring.
      return char.ToUpper(s[0]) + s.Substring(1);
    }
  }
}
