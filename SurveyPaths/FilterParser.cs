using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ITCLib;

namespace SurveyPaths
{
    public class FilterParser
    {
        // Stack and Lookaheads
        private Stack<DslToken> _tokenSequence;
        private DslToken _lookaheadFirst;
        private DslToken _lookaheadSecond;

        private List<List<FilterInstruction>> _scenarios { get; set; }
        private FilterInstruction _currentFilterInstruction;

        public List<List<FilterInstruction>> Parse(List<DslToken> tokens)
        {
           

            LoadSequenceStack(tokens);
            PrepareLookaheads();

            Match();

            DiscardToken(TokenType.SequenceTerminator);

            return _scenarios;
        }

        private void LoadSequenceStack(List<DslToken> tokens)
        {
            _tokenSequence = new Stack<DslToken>();
            int count = tokens.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                _tokenSequence.Push(tokens[i]);
            }
        }

        private void PrepareLookaheads()
        {
            _lookaheadFirst = _tokenSequence.Pop();
            _lookaheadSecond = _tokenSequence.Pop();
        }

        private DslToken ReadToken(TokenType tokenType)
        {
            if (_lookaheadFirst.TokenType != tokenType)
                throw new Exception(string.Format("Expected {0} but found: {1}", tokenType.ToString().ToUpper(), _lookaheadFirst.Value));

            return _lookaheadFirst;
        }

        private void DiscardToken()
        {
            _lookaheadFirst = _lookaheadSecond.Clone();

            if (_tokenSequence.Any())
                _lookaheadSecond = _tokenSequence.Pop();
            else
                _lookaheadSecond = new DslToken(TokenType.SequenceTerminator, string.Empty);
        }

        private void DiscardToken(TokenType tokenType)
        {
            if (_lookaheadFirst.TokenType != tokenType)
                throw new Exception(string.Format("Expected {0} but found: {1}", tokenType.ToString().ToUpper(), _lookaheadFirst.Value));

            DiscardToken();
        }

        private void Match()
        {
            DiscardToken(TokenType.AskIf);
            MatchCondition();
        }

        private void MatchCondition()
        {
            CreateNewMatchCondition();

            if (IsVarName(_lookaheadFirst))
            {
                if (IsEqualityOperator(_lookaheadSecond))
                {
                    //EqualityMatchCondition();
                }
                else if (_lookaheadSecond.TokenType == TokenType.NotEquals)
                {
                    //InCondition();
                }
                else if (_lookaheadSecond.TokenType == TokenType.GreaterThan)
                {
                    //NotInCondition();
                }
                else if (_lookaheadSecond.TokenType == TokenType.LessThan)
                {

                }
                else
                {
                    throw new Exception("Error" + " " + _lookaheadSecond.Value);
                }

                //MatchConditionNext();
            }
            else
            {
                throw new Exception("Error" + _lookaheadFirst.Value);
            }
        }

        //private void EqualityMatchCondition()
        //{
        //    _currentFilterInstruction.Object = GetObject(_lookaheadFirst);
        //    DiscardToken();
        //    _currentFilterInstruction.Operator = GetOperator(_lookaheadFirst);
        //    DiscardToken();
        //    _currentFilterInstruction.Value = _lookaheadFirst.Value;
        //    DiscardToken();
        //}

        //private DslObject GetObject(DslToken token)
        //{
        //    switch (token.TokenType)
        //    {
        //        case TokenType.Application:
        //            return DslObject.Application;
        //        case TokenType.ExceptionType:
        //            return DslObject.ExceptionType;
        //        case TokenType.Fingerprint:
        //            return DslObject.Fingerprint;
        //        case TokenType.Message:
        //            return DslObject.Message;
        //        case TokenType.StackFrame:
        //            return DslObject.StackFrame;
        //        default:
        //            throw new Exception(ExpectedObjectErrorText + token.Value);
        //    }
        //}

        private Operation GetOperator(DslToken token)
        {
            switch (token.TokenType)
            {
                case TokenType.Equals:
                    return Operation.Equals;
                case TokenType.NotEquals:
                    return Operation.NotEquals;
                case TokenType.GreaterThan:
                    return Operation.GreaterThan;
                case TokenType.LessThan:
                    return Operation.LessThan;
                default:
                    throw new Exception("Expected =, <>, >, < but found: " + token.Value);
            }
        }

        private void NotInCondition()
        {
            //ParseInCondition(DslOperator.NotIn);
        }

        private void InCondition()
        {
            //ParseInCondition(DslOperator.In);
        }

        //private void ParseInCondition(DslOperator inOperator)
        //{
        //    _currentFilterInstruction.Operator = inOperator;
        //    _currentFilterInstruction.Values = new List<string>();
        //    _currentFilterInstruction.Object = GetObject(_lookaheadFirst);
        //    DiscardToken();

        //    if (inOperator == DslOperator.In)
        //        DiscardToken(TokenType.In);
        //    else if (inOperator == DslOperator.NotIn)
        //        DiscardToken(TokenType.NotIn);

        //    DiscardToken(TokenType.OpenParenthesis);
        //    StringLiteralList();
        //    DiscardToken(TokenType.CloseParenthesis);
        //}

        //private void StringLiteralList()
        //{
        //    _currentFilterInstruction.Values.Add(ReadToken(TokenType.StringValue).Value);
        //    DiscardToken(TokenType.StringValue);
        //    StringLiteralListNext();
        //}

        //private void StringLiteralListNext()
        //{
        //    if (_lookaheadFirst.TokenType == TokenType.Comma)
        //    {
        //        DiscardToken(TokenType.Comma);
        //        _currentFilterInstruction.Values.Add(ReadToken(TokenType.StringValue).Value);
        //        DiscardToken(TokenType.StringValue);
        //        StringLiteralListNext();
        //    }
        //    else
        //    {
        //        // nothing
        //    }
        //}

        //private void MatchConditionNext()
        //{
        //    if (_lookaheadFirst.TokenType == TokenType.And)
        //    {
        //        AndMatchCondition();
        //    }
        //    else if (_lookaheadFirst.TokenType == TokenType.Or)
        //    {
        //        OrMatchCondition();
        //    }
        //    else if (_lookaheadFirst.TokenType == TokenType.Between)
        //    {
        //        DateCondition();
        //    }
        //    else
        //    {
        //        throw new Exception("Expected AND, OR or BETWEEN but found: " + _lookaheadFirst.Value);
        //    }
        //}

        //private void AndMatchCondition()
        //{
        //    _currentFilterInstruction.LogOpToNextCondition = DslLogicalOperator.And;
        //    DiscardToken(TokenType.And);
        //    MatchCondition();
        //}

        //private void OrMatchCondition()
        //{
        //    _currentFilterInstruction.LogOpToNextCondition = DslLogicalOperator.Or;
        //    DiscardToken(TokenType.Or);
        //    MatchCondition();
        //}

        //private void DateCondition()
        //{
        //    DiscardToken(TokenType.Between);

        //    _queryModel.DateRange = new DateRange();
        //    _queryModel.DateRange.From = DateTime.ParseExact(ReadToken(TokenType.DateTimeValue).Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        //    DiscardToken(TokenType.DateTimeValue);
        //    DiscardToken(TokenType.And);
        //    _queryModel.DateRange.To = DateTime.ParseExact(ReadToken(TokenType.DateTimeValue).Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        //    DiscardToken(TokenType.DateTimeValue);
        //    DateConditionNext();
        //}

        //private void DateConditionNext()
        //{
        //    if (_lookaheadFirst.TokenType == TokenType.Limit)
        //    {
        //        Limit();
        //    }
        //    else if (_lookaheadFirst.TokenType == TokenType.SequenceTerminator)
        //    {
        //        // nothing
        //    }
        //    else
        //    {
        //        throw new Exception("Expected LIMIT or the end of the query but found: " + _lookaheadFirst.Value);
        //    }

        //}

        //private void Limit()
        //{
        //    DiscardToken(TokenType.Limit);
        //    int limit = 0;
        //    bool success = int.TryParse(ReadToken(TokenType.Number).Value, out limit);
        //    if (success)
        //        _queryModel.Limit = limit;
        //    else
        //        throw new Exception("Expected an integer number but found " + ReadToken(TokenType.Number).Value);

        //    DiscardToken(TokenType.Number);
        //}

        private bool IsVarName(DslToken token)
        {
            return token.TokenType == TokenType.VarName;
                 
        }

        private bool IsEqualityOperator(DslToken token)
        {
            return token.TokenType == TokenType.Equals
                   || token.TokenType == TokenType.NotEquals
                   || token.TokenType == TokenType.GreaterThan
                   || token.TokenType == TokenType.LessThan;
        }

        private void CreateNewMatchCondition()
        {
            _currentFilterInstruction = new FilterInstruction();
            //_scenarios.Add(_currentFilterInstruction);
        }

        public List<string> Values(string filterExpression)
        {
            List<string> values = new List<string>();
            int varEnd = 0;
            for (int i = 0; i < filterExpression.Length; i++)
            {
                if (IsOperator(filterExpression[i]))
                {
                    varEnd = i;
                    break;
                }
            }

            string numbers = filterExpression.Substring(varEnd+1);
            numbers = numbers.Replace(".", "");
            if (Regex.IsMatch(numbers, "[0-9]+(,\\s[0-9]+)*\\sor\\s[0-9]+"))
            {
                numbers = numbers.Replace(",", "");
                numbers = numbers.Replace("or", "");

                values.AddRange(numbers.Split(new char[] { ' ' }));
            }
            else if (Regex.IsMatch(numbers, "[0-9]+\\-[0-9]+\\sor\\s[0-9]+"))
            {
                int lbound = Int32.Parse(numbers.Substring(0, numbers.IndexOf('-')));
                int ubound = Int32.Parse(numbers.Substring(numbers.IndexOf('-') + 1, numbers.Length - numbers.IndexOf('-')));


                for (int i = lbound; i <= ubound; i++)
                {
                    values.Add(i.ToString());
                }

                string rest = numbers.Substring(numbers.IndexOf("or ") + 3);
                values.Add(rest);
            }
            else if (Regex.IsMatch(numbers, "[0-9]+\\sor\\s[0-9]+"))
            {
                numbers = numbers.Replace("or", "");

                values.AddRange(numbers.Split(new char[] { ' ' }));
            }
            
            else if (Regex.IsMatch(numbers, "[0-9]+\\-[0-9]+"))
            {
                int lbound = Int32.Parse(numbers.Substring(0, numbers.IndexOf('-')));
                int ubound = Int32.Parse(numbers.Substring(numbers.IndexOf('-') + 1));

                for (int i = lbound; i <= ubound; i++)
                {
                    values.Add(i.ToString());
                }
            }
            else if (Regex.IsMatch(numbers, "[0-9]+\\sand\\s<[0-9]+"))
            {
                int lbound = Int32.Parse(numbers.Substring(0, numbers.IndexOf(' ')));
                int ubound = Int32.Parse(numbers.Substring(numbers.IndexOf('<') + 1));

                for (int i = lbound +1; i < ubound;i++)
                {
                    values.Add(i.ToString());
                }
            }
            else if (Regex.IsMatch(numbers, "[0-9]+\\sand\\s>[0-9]+"))
            {

                int ubound = Int32.Parse(numbers.Substring(0, numbers.IndexOf(' ')));
                int lbound = Int32.Parse(numbers.Substring(numbers.IndexOf('>') + 1));

                for (int i = lbound + 1; i < ubound; i++)
                {
                    values.Add(i.ToString());
                }
            }
            else if (Regex.IsMatch(numbers, "[0-9]+"))
            {
                values.Add(numbers);
            }
                // check what form we have
                // 
                // #, # or #
                // # or #
                // #

             

            return values;
        }

        private bool IsOperator(char c)
        {
            if (c == '=' || c == '<' || c == '>' || c == '!')
                return true;
            else
                return false;

        }
    }

    

    

    
}
