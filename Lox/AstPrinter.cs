using System.Text;

namespace Lox {
  public class AstPrinter : Expr.IExprVisitor<string> {
    public string Print(Expr expr) {
      return expr.Accept(this);
    }

    public string VisitBinaryExpr(Expr.Binary expr) {
      return Parenthesize(expr.op.Lexeme, expr.left, expr.right);
    }

    public string VisitGroupingExpr(Expr.Grouping expr) {
      return Parenthesize("group", expr.expression);
    }

    public string VisitLiteralExpr(Expr.Literal expr) {
      if(expr.value == null) return "nil";
      return expr.value.ToString();
    }

    public string VisitUnaryExpr(Expr.Unary expr) {
      return Parenthesize(expr.op.Lexeme, expr.right);
    }

    private string Parenthesize(string name, params Expr[] expressions) {
      StringBuilder builder = new StringBuilder();

      builder.Append("(").Append(name);
      foreach(Expr expr in expressions) {
        builder.Append(" ");
        builder.Append(expr.Accept(this));
      }
      builder.Append(")");

      return builder.ToString();
    }
  }
}