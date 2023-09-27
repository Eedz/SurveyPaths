using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITCLib;
using BooleanLogicParser;
using System.Text.RegularExpressions;

namespace SurveyPaths
{
    public class Timing
    {
        public string Title { get; set; }
        public string SurveyCode { get; set; }

        public Survey ReferenceSurvey { get; set; } // usually the previous wave
        public List<LinkedQuestion> Questions { get; set; }
        public List<LinkedQuestion> NotInterpreted { get; set; }


        public int WPM { get; set; }
        public double TotalTime { get; set; }
        public double TotalWeightedTime { get; set; }

        public int StartQ { get; set; } // Qnum value where we will start timing
        public bool IncludeNotes { get; set; }
        public bool SmartWordCount { get; set; }
        public string Notes { get; set;  }

        // default constructor
        public Timing()
        {
            Title = "New Timing Run";
            SurveyCode = "";
        
            Questions = new List<LinkedQuestion>();
            NotInterpreted = new List<LinkedQuestion>();

            WPM = 150;
            TotalTime = 0;
            TotalWeightedTime = 0;

            StartQ = 0;
            IncludeNotes = false;
            Notes = "";
        }

        

        public int QuestionCount()
        {

            return Questions.Count();

        }

        public int QuestionIndex(string refVarName)
        {
            for (int i = 0; i < Questions.Count(); i++)
            {
                if (Questions[i].VarName.RefVarName.Equals(refVarName))
                    return i;
            }
            return -1;
        }

        public LinkedQuestion QuestionAt(string refVarName)
        {
            for (int i = 0; i < Questions.Count(); i++)
            {
                if (Questions[i].VarName.RefVarName.ToLower().Equals(refVarName.ToLower()))
                    return Questions[i];
            }
            return null;
        }

        public LinkedQuestion QuestionAt(int index)
        {
            return Questions[index];
        }

        #region PreP Analysis
        /// <summary>
        /// For each question, get any VarNames appearing in the PreP and then get a reference to that question in the master list and
        /// add this reference to the list of filter questions.
        /// </summary>
        public void PopulateFilters()
        {
            // build a list of non-standard VarNames in the survey. These may appear in filters.
            List<string> nonStd = new List<string>();
            foreach (LinkedQuestion q in Questions)
            {
                if (!Regex.IsMatch(q.VarName.RefVarName, "[A-Z][A-Z][0-9][0-9][0-9][a-z]*"))
                    nonStd.Add(q.VarName.RefVarName);
            }

            foreach (LinkedQuestion q in Questions)
            {
                string prep = "";
                if (q.PreP.Contains(".")) 
                    prep = q.PreP.Substring(0, q.PreP.IndexOf("."));
                else
                    prep = q.PreP;


                // get the list of filter instructions for this question (both standard and non-standard VarNames)
                var filters = q.GetFilterInstructions();
                filters.AddRange(q.GetFilterInstructions(nonStd));

                // move on if there are no filters
                if (filters.Count == 0)
                    continue;

                foreach (FilterInstruction fi in filters)
                {
                    if (fi.VarName.Equals(q.VarName.RefVarName))
                        continue;

                    // add the SurveyQuestion represented by this filter instruction to the list of FilteredOn questions
                    LinkedQuestion filterVar = Questions.Find(x => x.VarName.RefVarName.Equals(fi.VarName));
                    if (filterVar != null && filterVar.GetQnumValue() <= q.GetQnumValue())
                    {
                        q.FilteredOn.Add(filterVar);
                    }
                }

                List<string> scenarios = new List<string>();

                // loop through filter instructions
                // if found in prep, replace with a letter until we are reduced to the form A AND B OR C 
                // add each filter instruction to the dictionary with its letter as the key
                // now go through the prep and try to create scenarios using the letters and AND/OR conditions
                // 
                // after the scenarios are constructed, replace the keys with the values


                prep = prep.Replace(" OR ", " or ");
                prep = prep.Replace("[", "(");
                prep = prep.Replace("]", ")");
                prep = prep.Replace("<>", "!");

                Dictionary<string, FilterInstruction> filterLookup = new Dictionary<string, FilterInstruction>();
                int c = 65;
                foreach (FilterInstruction f in filters)
                {
                    if (f.AnyOf)
                    {

                    }


                    // TODO some filters are contained in others
                    //if (prep.Contains("(" + f.ToString() + ")"))
                    //{

                    prep = Utilities.ReplaceFirstOccurrence(prep, f.ToString().Replace("<>", "!"), "$" + ((char)c).ToString() + "$");
                    //}
                    //else
                    //{                    
                    //    prep = Utilities.ReplaceFirstOccurrence(prep, f.ToString(), "($" + ((char)c).ToString() + "$)");
                    //}

                    filterLookup.Add("$" + ((char)c).ToString() + "$", f);
                    c++;
                }

                string[] prepLines = prep.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in prepLines)
                {
                    if (!line.StartsWith("Ask if"))
                        continue;

                    try
                    {
                        List<Token> tokens = GetTokens(line);

                        List<Token> postFix = PostFixUtils.infixToPostfixList(tokens);
                        List<string> cases = new List<string>();

                        cases = PostFixUtils.GetCases(PostFixUtils.getInfixCases(postFix));



                        foreach (string ca in cases)
                        {
                            List<FilterInstruction> l = new List<FilterInstruction>();

                            // extract the keys from ca and add to list
                            foreach (KeyValuePair<string, FilterInstruction> p in filterLookup)
                            {
                                if (ca.Contains(p.Key))
                                    l.Add(p.Value);
                            }

                            q.FilterList.Add(l);
                        }
                    }
                    catch (Exception)
                    {
                        int i = 0;
                    }
                }

                // look for uninterpretable filters
                if (q.FilteredOn.Count() > 0 && q.FilterList.Count == 0)
                    NotInterpreted.Add(q);
                else
                {

                    bool found = false;
                    foreach (LinkedQuestion fo in q.FilteredOn)
                    {
                        // if prep has a varname
                        // but there is no scenario for it, add to uninterpreted list
                        foreach (List<FilterInstruction> fi in q.FilterList)
                        {
                            foreach (FilterInstruction f in fi)
                            {
                                if (f.VarName.Equals(fo.VarName.RefVarName))
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (found) break;
                        }


                    }
                    if (!found)
                        NotInterpreted.Add(q);
                }
            }


        }
       

        private List<Token> GetTokens(string input)
        {
            Tokenizer tk = new Tokenizer(input);

            List<Token> tokens = tk.Tokenize().ToList();
            tokens.RemoveAll(item => item == null);

            // check if 
            // contains ()
            // contains (or or)
            // contains (and and)

            if (tokens.Count == 0)
                return tokens;

            string tokenString = string.Join("", tokens);

            bool invalidStart = false; bool invalidEnd = false; bool emptyParens = false;
            bool badOR = false, badAND = false;
            bool toFix;

            invalidStart = (tokenString.StartsWith("or") || tokenString.StartsWith("and") || tokenString.StartsWith(")"));

            invalidEnd = (tokenString.EndsWith("or") || tokenString.EndsWith("and") || tokenString.EndsWith("("));

            emptyParens = (tokenString.Contains("()"));

            badOR = (tokenString.Contains("(or") || tokenString.Contains("or)") || tokenString.Contains("oror"));
            badAND = (tokenString.Contains("(and") || tokenString.Contains("and)") || tokenString.Contains("andand"));

            toFix = invalidStart || invalidEnd || emptyParens || badOR || badAND;

            while (toFix)
            {
                // starts with OR AND closed bracket
                while (tokenString.StartsWith("or") || tokenString.StartsWith("and") || tokenString.StartsWith(")"))
                {
                    tokens[0] = null;
                    tokens.RemoveAll(item => item == null);
                    tokenString = string.Join("", tokens);
                }

                // ends with OR AND open bracket
                while (tokenString.EndsWith("or") || tokenString.EndsWith("and") || tokenString.EndsWith("("))
                {
                    tokens[tokens.Count - 1] = null;
                    tokens.RemoveAll(item => item == null);
                    tokenString = string.Join("", tokens);
                }

                // contains empty parentheses
                while (tokenString.Contains("()"))
                {
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        if (tokens[i] is OpenParenthesisToken && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i] = null;
                            tokens[i + 1] = null;
                        }
                    }
                    tokens.RemoveAll(item => item == null);
                    tokenString = string.Join("", tokens);
                }


                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i] is OrToken)
                    {
                        if (i > 0 && tokens[i - 1] is OpenParenthesisToken && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i - 1] = null;
                            tokens[i + 1] = null;
                            tokens[i] = null;
                        }
                        else if (i < tokens.Count - 1 && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i] = null;
                        }
                        else if (i > 0 && tokens[i - 1] is OpenParenthesisToken)
                        {
                            tokens[i] = null;
                        }
                        else if (i < tokens.Count - 1 && tokens[i + 1] is OrToken)
                        {
                            tokens[i + 1] = null;
                        }

                    }
                }

                tokens.RemoveAll(item => item == null);
                tokenString = string.Join("", tokens);

                for (int i = 0; i < tokens.Count; i++)
                {
                    if (tokens[i] is AndToken)
                    {
                        if (i > 0 && tokens[i - 1] is OpenParenthesisToken && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i - 1] = null;
                            tokens[i + 1] = null;
                            tokens[i] = null;
                        }
                        else if (i < tokens.Count - 1 && tokens[i + 1] is ClosedParenthesisToken)
                        {
                            tokens[i] = null;
                        }
                        else if (i > 0 && tokens[i - 1] is OpenParenthesisToken)
                        {
                            tokens[i] = null;
                        }
                        else if (i < tokens.Count - 1 && tokens[i + 1] is AndToken)
                        {
                            tokens[i + 1] = null;
                        }
                    }
                }
                tokens.RemoveAll(item => item == null);
                tokenString = string.Join("", tokens);

                while (Regex.IsMatch(tokenString, "\\(\\$[A-Z]\\$\\)"))
                {
                    // remove brackets around a variable ($A$)
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        if (tokens[i] is VariableToken)
                        {
                            if (i > 0 && tokens[i - 1] is OpenParenthesisToken && tokens[i + 1] is ClosedParenthesisToken)
                            {
                                tokens[i - 1] = null;
                                tokens[i + 1] = null;

                            }
                        }

                    }
                    tokens.RemoveAll(item => item == null);
                    tokenString = string.Join("", tokens);
                }


                invalidStart = (tokenString.StartsWith("or") || tokenString.StartsWith("and") || tokenString.StartsWith(")"));
                invalidEnd = (tokenString.EndsWith("or") || tokenString.EndsWith("and") || tokenString.EndsWith("("));
                emptyParens = (tokenString.Contains("()"));

                badOR = (tokenString.Contains("(or") || tokenString.Contains("or)") || tokenString.Contains("oror"));
                badAND = (tokenString.Contains("(and") || tokenString.Contains("and)") || tokenString.Contains("andand"));

                toFix = invalidStart || invalidEnd || emptyParens || badOR || badAND;
            }

            return tokens;
        }

        #endregion

        #region PstP Analysis


        /// <summary>
        /// (QuestionRouting) For each question, get any VarNames appearing in the PstP and then get a reference to that question in the master list and
        /// add this reference to the list of possible next questions.
        /// </summary>
        private void PopulateNextQuestions()
        {

            foreach (LinkedQuestion q in Questions)
            {
                var qr = new QuestionRouting(q.PstP, q.RespOptions);

                foreach (RoutingVar rv in qr.RoutingVars)
                {
                    string s = rv.Varname.Substring(0, rv.Varname.IndexOf("."));
                    LinkedQuestion next = Questions.Find(x => x.VarName.VarName.Equals(s));

                    foreach (int v in rv.ResponseCodes)
                        q.PossibleNext.Add(v, next);
                }
            }
        }

        /// <summary>
        /// (string) For each question, get any varnames appearing in the PstP and then get a reference to that question in the master list and
        /// add this reference to the list of possible next questions.
        /// </summary>
        private void PopulateNextQuestionsString()
        {
            foreach (LinkedQuestion q in Questions)
            {
                int i = 0;
                var routing = q.GetRoutingVars();

                foreach (string s in routing)
                {
                    LinkedQuestion next = Questions.Find(x => x.VarName.VarName.Equals(s));

                    q.PossibleNext.Add(i, next);
                    i++;
                }
            }
        }
        #endregion

       

        public override string ToString()
        {
            return Title;
        }
    }
}
