using System;

namespace Lox {
  public class Token {
    public TokenType Type { get; }
    public string Lexeme { get; }
    public Object Literal { get; }
    public int Line { get; }
    
    public Token(TokenType type, string lexeme, Object literal, int line) {
      this.Type = type;
      this.Lexeme = lexeme;
      this.Literal = literal;
      this.Line = line;
    }

    public override string ToString() {
      return $"Type: {Type} - Lexeme: {Lexeme} - Literal: {Literal}";
    }
  }
}