﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class UnorderedListMacro : INestedBlockMacro
    {
        private static readonly Regex LevelRegex = new Regex(@"\*", RegexOptions.Compiled);

        public string Id
        {
            get { return "UnorderedList"; }
        }

        public string BlockStartScope
        {
            get { return ScopeName.UnorderedListBeginTag; }
        }

        public string BlockEndScope
        {
            get { return ScopeName.UnorderedListEndTag; }
        }

        public string ItemStartScope
        {
            get { return ScopeName.ListItemBegin; }
        }

        public string ItemEndScope
        {
            get { return ScopeName.ListItemEnd; }
        }

        public Func<string, int> DetermineLevel
        {
            get { return x => LevelRegex.Matches(x).Count; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(EscapeRegexPatterns.FullEscape),
                               new MacroRule(
                                   @"(^\*+\s)[^\r\n]+((?:\r\n)?)$",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.ListItemBegin},
                                           {2, ScopeName.ListItemEnd}
                                       })
                           };
            }
        }
    }
}