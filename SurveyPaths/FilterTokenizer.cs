using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITCLib;
using System.Text.RegularExpressions;

namespace SurveyPaths
{
    public class FilterTokenizer
    {

        private List<TokenDefinition> _tokenDefinitions;
        // make a token for each type of expression
        public FilterTokenizer()
        {
            _tokenDefinitions = new List<TokenDefinition>();

            _tokenDefinitions.Add(new TokenDefinition(TokenType.AskIf, "^Ask if"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.VarEqualsValues, "^[A-Z][A-Z]\\d\\d\\d=\\d+(,\\s\\d|\\sor\\s\\d)*"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.VarGreaterThanValues, "^[A-Z][A-Z]\\d\\d\\d>\\d+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.VarLessThanValues, "^[A-Z][A-Z]\\d\\d\\d<\\d+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.VarBetweenValues, "^[A-Z][A-Z]\\d\\d\\d>\\d+\\sand\\s<\\d+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.VarNotEqualValues, "^[A-Z][A-Z]\\d\\d\\d<>\\d+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.AnyOfEqualsValue, "^any\\sof\\s(\\([A-Z][A-Z]\\d\\d\\d(,[A-Z][A-Z]\\d\\d\\d)*\\))=\\d+"));

            
            _tokenDefinitions.Add(new TokenDefinition(TokenType.And, "^and"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.VarName, "^[A-Z][A-Z]\\d\\d\\d"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Dash, "^-"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.CloseParenthesis, "^\\)"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Comma, "^,"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Equals, "^="));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.NotEquals, "^<>"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.OpenParenthesis, "^\\("));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Or, "^or"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.StringValue, "^'[^']*'"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.Number, "^\\d+"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.GreaterThan, "^>"));
            _tokenDefinitions.Add(new TokenDefinition(TokenType.LessThan, "^<"));
        }

        public IEnumerable<DslToken> Tokenize(string lqlText)
        {
            var tokens = new List<DslToken>();

            string remainingText = lqlText;

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);
                if (match.IsMatch)
                {
                    tokens.Add(new DslToken(match.TokenType, match.Value));
                    remainingText = match.RemainingText;
                }
                else
                {
                    if (IsWhitespace(remainingText))
                    {
                        remainingText = remainingText.Substring(1);
                    }
                    else
                    {
                        var invalidTokenMatch = CreateInvalidTokenMatch(remainingText);
                        tokens.Add(new DslToken(invalidTokenMatch.TokenType, invalidTokenMatch.Value));
                        remainingText = invalidTokenMatch.RemainingText;
                    }
                }
            }

            tokens.Add(new DslToken(TokenType.SequenceTerminator, string.Empty));

            return tokens;
        }

        private TokenMatch FindMatch(string lqlText)
        {
            foreach (var tokenDefinition in _tokenDefinitions)
            {
                var match = tokenDefinition.Match(lqlText);
                if (match.IsMatch)
                    return match;
            }

            return new TokenMatch() { IsMatch = false };
        }

        private bool IsWhitespace(string lqlText)
        {
            return Regex.IsMatch(lqlText, "^\\s+");
        }

        private TokenMatch CreateInvalidTokenMatch(string lqlText)
        {
            var match = Regex.Match(lqlText, "(^\\S+\\s)|^\\S+");
            if (match.Success)
            {
                return new TokenMatch()
                {
                    IsMatch = true,
                    RemainingText = lqlText.Substring(match.Length),
                    TokenType = TokenType.Invalid,
                    Value = match.Value.Trim()
                };
            }

            throw new Exception("Failed to generate invalid token");
        }

    }

    public class TokenDefinition
    {
        private Regex _regex;
        private readonly TokenType _returnsToken;

        public TokenDefinition(TokenType returnsToken, string regexPattern)
        {
            _regex = new Regex(regexPattern, RegexOptions.IgnoreCase);
            _returnsToken = returnsToken;
        }

        public TokenMatch Match(string inputString)
        {
            var match = _regex.Match(inputString);
            if (match.Success)
            {
                string remainingText = string.Empty;
                if (match.Length != inputString.Length)
                    remainingText = inputString.Substring(match.Length);

                return new TokenMatch()
                {
                    IsMatch = true,
                    RemainingText = remainingText,
                    TokenType = _returnsToken,
                    Value = match.Value
                };
            }
            else
            {
                return new TokenMatch() { IsMatch = false };
            }

        }
    }

    public class TokenMatch
    {
        public bool IsMatch { get; set; }
        public TokenType TokenType { get; set; }
        public string Value { get; set; }
        public string RemainingText { get; set; }
    }

    public class DslToken
    {
        public DslToken(TokenType tokenType)
        {
            TokenType = tokenType;
            Value = string.Empty;
        }

        public DslToken(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value = value;
        }

        public TokenType TokenType { get; set; }
        public string Value { get; set; }

        public DslToken Clone()
        {
            return new DslToken(TokenType, Value);
        }
    }

    public enum TokenType
    {
        AskIf,
        VarEqualsValues,
        VarGreaterThanValues,
        VarLessThanValues,
        VarBetweenValues,
        VarNotEqualValues,
        AnyOfEqualsValue,

        NotDefined,
        And,
        VarName,
        SingleValue,
        TwoValue,
        ListValue,
        RangeValue,
        AnyOf,
        Dash,
        CloseParenthesis,
        Comma,
        Equals,
        NotEquals,
        Number,
        Or,
        OpenParenthesis,
        StringValue,
        SequenceTerminator,
        Invalid,
        GreaterThan,
        LessThan
    }
}
