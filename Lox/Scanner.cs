using System;
using System.Collections.Generic;

namespace Lox {
  public class Scanner {
    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();

    private int start = 0;
    private int current = 0;
    private int line = 1;

    private static readonly Dictionary<String, TokenType> keywords;

    static Scanner() {
      keywords = new Dictionary<string, TokenType>();
      keywords.Add("and", TokenType.AND);
      keywords.Add("class", TokenType.CLASS);
      keywords.Add("else", TokenType.ELSE);
      keywords.Add("false", TokenType.FALSE);
      keywords.Add("for", TokenType.FOR);
      keywords.Add("fun", TokenType.FUN);
      keywords.Add("if", TokenType.IF);
      keywords.Add("nil", TokenType.NIL);
      keywords.Add("or", TokenType.OR);
      keywords.Add("print", TokenType.PRINT);
      keywords.Add("return", TokenType.RETURN);
      keywords.Add("super", TokenType.SUPER);
      keywords.Add("this", TokenType.THIS);
      keywords.Add("true", TokenType.TRUE);
      keywords.Add("var", TokenType.VAR);
      keywords.Add("while", TokenType.WHILE);
    }

    public Scanner(string source) {
      this.source = source;
    }

    public List<Token> ScanTokens() {
      while (!IsAtEnd()) {
        start = current;
        ScanToken();
      }

      tokens.Add(new Token(TokenType.EOF, "", null, line));
      return tokens;
    }

    private bool IsAtEnd() {
      return current >= source.Length;
    }

    private void ScanToken() {
      char c = Advance();

      switch (c) {
        case '(': AddToken(TokenType.LEFT_PAREN); break;
        case ')': AddToken(TokenType.RIGHT_PAREN); break;
        case '{': AddToken(TokenType.LEFT_BRACE); break;
        case '}': AddToken(TokenType.RIGHT_BRACE); break;
        case ',': AddToken(TokenType.COMMA); break;
        case '.': AddToken(TokenType.DOT); break;
        case '-': AddToken(TokenType.MINUS); break;
        case '+': AddToken(TokenType.PLUS); break;
        case ';': AddToken(TokenType.SEMICOLON); break;
        case '*': AddToken(TokenType.STAR); break;

        case '!':
          AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
          break;
        case '=':
          AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
          break;
        case '<':
          AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
          break;
        case '>':
          AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
          break;
        case '/':
          if (Match('/')) {
            while (Peek() != '\n' && !IsAtEnd()) Advance();
          } else if (Match('*')) {
            while (Peek() != '*' && PeekNext() != '/') Advance();
            SkipBytes(2);
          } else {
            AddToken(TokenType.SLASH);
          }
          break;

        case '"': String(); break;

        case ' ':
        case '\r':
        case '\t':
          break;
        case '\n':
          line++;
          break;

        default:
          if (IsDigit(c))
            Number();
          else if (IsAlpha(c))
            Identifier();
          else
            Lox.Error(line, "Unexpected character");
          break;
      }
    }

    private bool Match(char expected) {
      if (IsAtEnd()) return false;
      if (source[current] != expected) return false;

      current++;
      return true;
    }

    private char Peek() {
      if (IsAtEnd()) return '\0';
      return source[current];
    }

    private char PeekNext() {
      if (current + 1 >= source.Length) return '\0';
      return source[current + 1];
    }

    private void SkipBytes(int count) {
      current += count;
    }

    private char Advance() {
      return source[current++];
    }

    private bool IsDigit(char c) {
      return c >= '0' && c <= '9';
    }

    private bool IsAlpha(char c) {
      return (c >= 'a' && c <= 'z') ||
             (c >= 'A' && c <= 'Z') ||
              c == '_';
    }

    private bool IsAlphaNumeric(char c) {
      return IsAlpha(c) || IsDigit(c);
    }

    private void Identifier() {
      while (IsAlphaNumeric(Peek())) Advance();

      String text = source.Substring(start, current - start);
      TokenType type = keywords.GetValueOrDefault(text, TokenType.IDENTIFIER);

      AddToken(type);
    }

    private void Number() {
      while (IsDigit(Peek())) Advance();

      if (Peek() == '.' && IsDigit(PeekNext())) {
        Advance();

        while (IsDigit(Peek())) Advance();
      }

      AddToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current - start)));
    }

    private void String() {
      while (Peek() != '"' && !IsAtEnd()) {
        if (Peek() == '\n') line++;
        Advance();
      }

      if (IsAtEnd()) {
        Lox.Error(line, "Unterminated string");
      }

      Advance();

      String value = source.Substring(start + 1, current - start - 2);
      AddToken(TokenType.STRING, value);
    }

    private void AddToken(TokenType type) {
      AddToken(type, null);
    }

    private void AddToken(TokenType type, Object literal) {
      string text = source.Substring(start, current - start);
      tokens.Add(new Token(type, text, literal, line));
    }
  }
}