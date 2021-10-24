using System;

namespace Lox
{
  abstract class Expr
  {
    class Binary : Expr
    {
      readonly Expr left;
      readonly Token op;
      readonly Expr right;

      Binary(Expr left, Token op, Expr right)
      {
        this.left = left;
        this.op = op;
        this.right = right;
      }
    }
    class Grouping : Expr
    {
      readonly Expr expression;

      Grouping(Expr expression)
      {
        this.expression = expression;
      }
    }
    class Literal : Expr
    {
      readonly Object value;

      Literal(Object value)
      {
        this.value = value;
      }
    }
    class Unary : Expr
    {
      readonly Token op;
      readonly Expr right;

      Unary(Token op, Expr right)
      {
        this.op = op;
        this.right = right;
      }
    }
  }
}
