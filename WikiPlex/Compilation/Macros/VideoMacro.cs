using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
    public class VideoMacro : IMacro
    {
        public string Id
        {
            get { return "Video"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>
                           {
                               new MacroRule(
                                   @"(?i)(\{video\:)([^\}]*type=Flash[^\}]*)(\})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.FlashVideo},
                                           {3, ScopeName.Remove}
                                       }),
                                new MacroRule(
                                   @"(?i)(\{video\:)([^\}]*type=QuickTime[^\}]*)(\})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.QuickTimeVideo},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)(\{video\:)([^\}]*type=Real[^\}]*)(\})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.RealPlayerVideo},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)(\{video\:)([^\}]*type=Windows[^\}]*)(\})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.WindowsMediaVideo},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)(\{video\:)([^\}]*type=YouTube[^\}]*)(\})",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.YouTubeVideo},
                                           {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)(\{video\:)([^\}]*type=(?:C9|Channel9)[^\}]*)(\})",
                                   new Dictionary<int, string>
                                       {
                                          {1, ScopeName.Remove},
                                          {2, ScopeName.Channel9Video},
                                          {3, ScopeName.Remove}
                                       }),
                               new MacroRule(
                                   @"(?i)\{video\:[^\}]*[^\}]*\}",
                                   new Dictionary<int, string>
                                       {
                                           {0, ScopeName.InvalidVideo}
                                       })
                           };
            }
        }
    }
}