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
      public readonly Expr left;
      public readonly Token op;
      public readonly Expr right;

      public Binary(Expr left, Token op, Expr right)
      {
        this.left = left;
        this.op = op;
        this.right = right;
      }

      public override T Accept<T>(IExprVisitor<T> visitor)
      {
        return visitor.VisitBinaryExpr(this);
      }
    }

    public class Grouping : Expr
    {
      public readonly Expr expression;

      public Grouping(Expr expression)
      {
        this.expression = expression;
      }

      public override T Accept<T>(IExprVisitor<T> visitor)
      {
        return visitor.VisitGroupingExpr(this);
      }
    }

    public class Literal : Expr
    {
      public readonly Object value;

      public Literal(Object value)
      {
        this.value = value;
      }

      public override T Accept<T>(IExprVisitor<T> visitor)
      {
        return visitor.VisitLiteralExpr(this);
      }
    }

    public class Unary : Expr
    {
      public readonly Token op;
      public readonly Expr right;

      public Unary(Token op, Expr right)
      {
        this.op = op;
        this.right = right;
      }

      public override T Accept<T>(IExprVisitor<T> visitor)
      {
        return visitor.VisitUnaryExpr(this);
      }
    }

    public abstract T Accept<T>(IExprVisitor<T> visitor);
  }
}
