using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WikiPlex.Common;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Compilation
{
    public class MacroCompiler : IMacroCompiler
    {
        private static readonly Regex numberOfCapturesRegex = new Regex(@"(?x)(?<!\\)\((?!\?)", RegexOptions.Compiled);
        private readonly Dictionary<string, CompiledMacro> compiledMacros;
        private readonly ReaderWriterLockSlim compileLock;

        public MacroCompiler()
        {
            compiledMacros = new Dictionary<string, CompiledMacro>();
            compileLock = new ReaderWriterLockSlim();
        }

        public CompiledMacro Compile(IMacro macro)
        {
            Guard.NotNull(macro, "macro");
            Guard.NotNullOrEmpty(macro.Id, "macro", "The macro identifier must not be null or empty.");

            try
            {
                // for performance sake, 
                // we'll initially use only a read lock
                compileLock.EnterReadLock();
                CompiledMacro compiledMacro = null;
                if (compiledMacros.TryGetValue(macro.Id, out compiledMacro))
                    return compiledMacro;
            }
            finally
            {
                compileLock.ExitReadLock();
            }

            // this is assuming the compiled macro was not found
            // and will attempt to compile it.
            compileLock.EnterUpgradeableReadLock();
            try
            {
                if (compiledMacros.ContainsKey(macro.Id))
                    return compiledMacros[macro.Id];

                compileLock.EnterWriteLock();
                try
                {
                    if (compiledMacros.ContainsKey(macro.Id))
                        return compiledMacros[macro.Id];

                    Guard.NotNullOrEmpty(macro.Rules, "macro", "The macro rules must not be null or empty.");

                    CompiledMacro compiledMacro = CompileMacro(macro);
                    compiledMacros[macro.Id] = compiledMacro;
                    return compiledMacro;
                }
                finally
                {
                    compileLock.ExitWriteLock();
                }
            }
            finally
            {
                compileLock.ExitUpgradeableReadLock();
            }
        }

        private CompiledMacro CompileMacro(IMacro macro)
        {
            Regex regex;
            IList<string> captures;

            CompileRules(macro.Rules, out regex, out captures);

            return new CompiledMacro(macro.Id, regex, captures);
        }

        private void CompileRules(IList<MacroRule> rules, out Regex regex, out IList<string> captures)
        {
            var regexBuilder = new StringBuilder();
            captures = new List<string>();

            regexBuilder.AppendLine("(?x)");
            captures.Add(null);

            CompileRule(rules[0], regexBuilder, captures, true);

            for (int i = 1; i < rules.Count; i++)
                CompileRule(rules[i], regexBuilder, captures, false);

            regex = new Regex(regexBuilder.ToString(), RegexOptions.Compiled);
        }

        private void CompileRule(MacroRule rule, StringBuilder regex, IList<string> captures, bool isFirstRule)
        {
            if (!isFirstRule)
            {
                regex.AppendLine();
                regex.AppendLine();
                regex.AppendLine("|");
                regex.AppendLine();
            }

            regex.AppendFormat("(?-xis)(?m)({0})(?x)", rule.Regex);

            int numberOfCaptures = GetNumberOfCaptures(rule.Regex);

            for (int i = 0; i <= numberOfCaptures; i++)
            {
                string scope = null;

                foreach (int captureIndex in rule.Captures.Keys)
                {
                    if (i == captureIndex)
                    {
                        scope = rule.Captures[captureIndex];
                        break;
                    }
                }

                captures.Add(scope);
            }
        }

        private static int GetNumberOfCaptures(string regex)
        {
            return numberOfCapturesRegex.Matches(regex).Count;
        }

        public void Flush()
        {
            compileLock.EnterWriteLock();
            try
            {
                compiledMacros.Clear();
            }
            finally
            {
                compileLock.ExitWriteLock();
            }
        }
    }
}