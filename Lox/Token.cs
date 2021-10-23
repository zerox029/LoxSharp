namespace Lox {
  public class Token {
    readonly TokenType Type { get; }
    readonly string Lexeme { get; }
    readonly Object Literal { get; }
    readonly int Line { get; }
    
    public Token(TokenType type, string lexeme, Object literal, int line) {
      this.Type = type;
      this.Lexeme = lexeme;
      this.Literal = literal;
      this.Line = line;
    }

    public string ToString() {
      return Type + " " + Lexeme + " " + Literal;
    }
  }
}