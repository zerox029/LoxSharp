using System.Collections.Generic;
using System;

namespace Lox {
  public class Parser {
    private readonly List<Token> tokens;
    private int current = 0;

    public Parser(List<Token> tokens) {
      this.tokens = tokens;
    }

    private Expr Expression() {
      return Equality();
    }

    private Expr Equality() {
      Expr expr = Comparison();

      while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL)) {
        Token op = Previous();
        Expr right = Comparison();
        expr = new Expr.Binary(expr, op, right);
      }

      return expr;
    }

    private Expr Comparison() {
      Expr expr = Term();

      while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL)) {
        Token op = Previous();
        Expr right = Term();
        expr = new Expr.Binary(expr, op, right);
      }

      return expr;
    }

    private Expr Term() {
      Expr expr = Factor();

      while (Match(TokenType.MINUS, TokenType.PLUS)) {
        Token op = Previous();
        Expr right = Factor();
        expr = new Expr.Binary(expr, op, right);
      }

      return expr;
    }

    private Expr Factor() {
      Expr expr = Unary();

      while (Match(TokenType.SLASH, TokenType.STAR)) {
        Token op = Previous();
        Expr right = Unary();
        expr = new Expr.Binary(expr, op, right);
      }

      return expr;
    }

    private Expr Unary() {
      if (Match(TokenType.BANG, TokenType.MINUS)) {
        Token op = Previous();
        Expr right = Unary();
        return new Expr.Unary(op, right);
      }

      return Primary();
    }

    private Expr Primary() {
      if (Match(TokenType.FALSE)) return new Expr.Literal(false);
      if (Match(TokenType.TRUE)) return new Expr.Literal(true);
      if (Match(TokenType.NIL)) return new Expr.Literal(null);

      if (Match(TokenType.NUMBER, TokenType.STRING)) {
        return new Expr.Literal(Previous().Literal);
      }

      if (Match(TokenType.LEFT_PAREN)) {
        Expr expr = Expression();
        Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
        return new Expr.Grouping(expr);
      }
    }

    private bool Match(params TokenType[] types) {
      foreach (TokenType type in types) {
        if (Check(type)) {
          Advance();
          return true;
        }
      }

      return false;
    }

    private bool Check(TokenType type) {
      if (IsAtEnd()) return false;
      return Peek().Type == type;
    }

    private Token Advance() {
      if (!IsAtEnd()) current++;
      return Previous();
    }

    private bool IsAtEnd() {
      return Peek().Type == TokenType.EOF;
    }

    private Token Peek() {
      return tokens[current];
    }

    private Token Previous() {
      return tokens[current - 1];
    }

    private void Consume(TokenType type, string errorMessage) {
      throw new NotImplementedException();
    }
  }
}