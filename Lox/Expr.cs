using System;

namespace Lox
{
  public abstract class Expr
  {
    public interface IExprVisitor<T>
    {
      T VisitBinaryExpr(Expr.Binary expr);
      T VisitGroupingExpr(Expr.Grouping expr);
      T VisitLiteralExpr(Expr.Literal expr);
      T VisitUnaryExpr(Expr.Unary expr);
    }

    public class Binary : Expr
    {
      public Expr Left { get; }
      public Token Op { get; }
      public Expr Right { get; }

      public Binary(Expr left, Token op, Expr right)
      {
        this.Left = left;
        this.Op = op;
        this.Right = right;
      }

      public override T Accept<T>(IExprVisitor<T> visitor)
      {
        return visitor.VisitBinaryExpr(this);
      }
    }

    public class Grouping : Expr
    {
      public Expr Expression { get; }

      public Grouping(Expr expression)
      {
        this.Expression = expression;
      }

      public override T Accept<T>(IExprVisitor<T> visitor)
      {
        return visitor.VisitGroupingExpr(this);
      }
    }

    public class Literal : Expr
    {
      public Object Value { get; }

      public Literal(Object value)
      {
        this.Value = value;
      }

      public override T Accept<T>(IExprVisitor<T> visitor)
      {
        return visitor.VisitLiteralExpr(this);
      }
    }

    public class Unary : Expr
    {
      public Token Op { get; }
      public Expr Right { get; }

      public Unary(Token op, Expr right)
      {
        this.Op = op;
        this.Right = right;
      }

      public override T Accept<T>(IExprVisitor<T> visitor)
      {
        return visitor.VisitUnaryExpr(this);
      }
    }

    public abstract T Accept<T>(IExprVisitor<T> visitor);
  }
}
