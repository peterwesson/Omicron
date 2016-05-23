using System;
using System.Collections.Generic;
using System.Linq;

using Omicron.Analysis.LexicalAnalysis.Tokens;
using Omicron.Exceptions;
using Omicron.Lanuage;
using Omicron.LexicalAnalysis.Tokens;

namespace Omicron.Analysis.LexicalAnalysis
{
    public class LexicalAnalyser : BaseAnalyser<string, IEnumerable<Token>>
    {
        private string _input;

        private bool _done;

        private int _currentLine;

        private bool IsAtEnd
        {
            get
            {
                return _input.Length == 0;
            }
        }

        public override IEnumerable<Token> Parse(string input)
        {
            _input = input;
            _currentLine = 1;

            var tokens = new List<Token>();

            while (!_done)
            {
                var token = Advance();
                if (token != null)
                {
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        private Token Advance()
        {
            EatWhitespace();

            if (IsAtEnd)
            {
                _done = true;
                return null;
            }

            var nextChar = NextChar();
            var nextTwoChars = nextChar + LookAhead().ToString();

            if (Syntax.IsOperator(nextTwoChars))
            {
                NextChar();
                return new Token(TokenType.Operator, nextTwoChars, _currentLine);
            }
            if (Syntax.IsOperator(nextChar.ToString()))
            {
                return new Token(TokenType.Operator, nextChar.ToString(), _currentLine);
            }
            if (Syntax.IsNumber(nextChar))
            {
                var intConst = nextChar.ToString();
                intConst += EatWhile(Syntax.IsNumber);

                int result;
                if (!int.TryParse(intConst, out result))
                {
                    throw new CompilationException(string.Format("Int const must be in range [0,2147483648), but got: {0}", intConst), _currentLine);
                }

                return new Token(TokenType.IntConst, intConst, _currentLine);
            }
            if (Syntax.IsCharOrdinalStart(nextChar))
            {
                var marker = NextChar();
                if (marker == '\\')
                {
                    var code = EatWhile(Syntax.IsNumber);
                    if (code.Length != 3)
                    {
                        throw new CompilationException("Expected: \\nnn where n are decimal digits", _currentLine);
                    }
                    var value = int.Parse(code);
                    if (value >= 256)
                    {
                        throw new CompilationException("Character ordinal is out of range [0,255]", _currentLine);
                    }
                    return new Token(TokenType.IntConst, value.ToString(), _currentLine);
                }

                NextChar();

                return new Token(TokenType.IntConst, ((int)marker).ToString(), _currentLine);
            }
            if (Syntax.IsStringConstantStart(nextChar))
            {
                var strConst = EatWhile(c => !Syntax.IsStringConstantStart(c));
                NextChar();
                return new Token(TokenType.StrConst, strConst, _currentLine);
            }
            if (Syntax.IsStartOfKeywordOrIdent(nextChar))
            {
                var keywordOrIdent = nextChar.ToString();
                keywordOrIdent += EatWhile(Syntax.IsPartOfKeywordOrIdent);

                return Syntax.IsKeyword(keywordOrIdent) ?
                    new Token(TokenType.Keyword, keywordOrIdent, _currentLine) :
                    new Token(TokenType.Identifier, keywordOrIdent, _currentLine);

            }
            if (Syntax.IsBracket(nextChar))
            {
                return new Token(TokenType.Bracket, nextChar.ToString(), _currentLine);
            }
            if (Syntax.IsSemiColon(nextChar))
            {
                return new Token(TokenType.SemiColon, nextChar.ToString(), _currentLine);
            }

            throw new CompilationException(string.Format("Unexpected character: {0}", nextChar), _currentLine);
        }

        private string EatWhile(Func<char, bool> predicate)
        {
            var word = string.Empty;

            while (predicate.Invoke(LookAhead()))
            {
                word += NextChar();
            }

            return word;
        }

        private void EatWhitespace()
        {
            while (Syntax.IsWhitespace(LookAhead()))
            {
                
                var character = NextChar();
                var nextCharacter = LookAhead();

                if ((character + nextCharacter.ToString()).Contains(Environment.NewLine))
                {
                    _currentLine++;
                }
            }
        }

        private char NextChar()
        {
            var character = _input.FirstOrDefault();

            _input = _input.Substring(1, _input.Length - 1);

            return character;
        }

        private char LookAhead()
        {
            return _input.FirstOrDefault();
        }
    }
}
