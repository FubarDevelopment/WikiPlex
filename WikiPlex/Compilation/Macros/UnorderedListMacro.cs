﻿using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    /// <summary>
    /// This macro will output an unordered list.
    /// </summary>
    /// <example><![CDATA[
    /// * item 1
    /// * item 2
    /// ** item 2.1
    /// *** item 2.1.1
    /// ]]></example>
    public class UnorderedListMacro : IListMacro
    {
        /// <summary>
        /// Gets the id of the macro.
        /// </summary>
        public string Id
        {
            get { return "UnorderedList"; }
        }

        /// <summary>
        /// Gets the list start scope name.
        /// </summary>
        public string ListStartScopeName
        {
            get { return ScopeName.UnorderedListBeginTag; }
        }

        /// <summary>
        /// Gets the list end scope name.
        /// </summary>
        public string ListEndScopeName
        {
            get { return ScopeName.UnorderedListEndTag; }
        }

        /// <summary>
        /// Gets the char used for depth (level) inspection.
        /// </summary>
        public char DepthChar
        {
            get { return '*'; }
        }

        /// <summary>
        /// Gets the list of rules for the macro.
        /// </summary>
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