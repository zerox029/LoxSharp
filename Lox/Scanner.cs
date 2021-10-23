namespace Lox {
  public class Scanner {
    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();

    private int start = 0;
    private int current = 0;
    private int line = 1;

    public Scanner(string source) {
      this.source = source;
    }

    public List<Token> ScanTokens() {
      while(!isAtEnd) {
        start = current;
        ScanToken();
      }

      tokens.Add(new Token(TokenType.EOF, "", null, line));
      return tokens;
    }

    private bool isAtEnd() {
      return current >= source.Length();
    }
  }
}