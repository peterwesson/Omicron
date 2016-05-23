using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Omicron.Lanuage.Operators;

namespace Omicron.Lanuage
{
    public static class Syntax
    {
        public static IEnumerable<string> GetOperators()
        {
            return GetOperators(o => true);
        }

        public static IEnumerable<string> GetOperators(Func<OperatorAttribute, bool> predicate)
        {
            return GetOperatorAttributes(predicate).Select(o => o.Symbol);
        }

        public static IEnumerable<OperatorAttribute> GetOperatorAttributes()
        {
            return GetOperatorAttributes(o => true);
        }


        public static IEnumerable<OperatorAttribute> GetOperatorAttributes(Func<OperatorAttribute, bool> predicate)
        {
            return Enum.GetValues(typeof(Operator)).OfType<Operator>()
                .Select(o =>
                    (OperatorAttribute)(typeof(Operator)
                    .GetMember(o.ToString())[0].GetCustomAttributes(typeof(OperatorAttribute), false).First())).Where(predicate);
        }

        private static readonly IEnumerable<string> Keywords = new List<string>
        {
            "else",
            "for",
            "if",
            "int",
            "return",
            "void",
            "while"
        };

        public static bool IsOperator(string word)
        {
            return GetOperators().Contains(word);
        }

        public static bool IsStartOfNumber(char character)
        {
            return Regex.IsMatch(character.ToString(), "[0-9-]");
        }

        public static bool IsNumber(char character)
        {
            return Regex.IsMatch(character.ToString(), "[0-9]");
        }

        public static bool IsStringConstantStart(char character)
        {
            return (character == '"');
        }

        public static bool IsStartOfKeywordOrIdent(char character)
        {
            return Regex.IsMatch(character.ToString(), "[A-Za-z_]");
        }

        public static bool IsPartOfKeywordOrIdent(char character)
        {
            return Regex.IsMatch(character.ToString(), "[A-Za-z0-9_]");
        }

        public static bool IsKeyword(string word)
        {
            return Keywords.Contains(word);
        }

        public static bool IsCharOrdinalStart(char character)
        {
            return false;
        }

        public static bool IsWhitespace(char character)
        {
            switch (character)
            {
                case ' ':
                case '\r':
                case '\n':
                case '\t':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsSemiColon(char character)
        {
            return character == ';';
        }

        public static bool IsBracket(char character)
        {
            switch (character)
            {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                    return true;
                default:
                    return false;
            }
        }
    }
}
