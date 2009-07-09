using System.Collections.Generic;
using WikiPlex.Common;

namespace WikiPlex.Compilation.Macros
{
    public class ImageMacro : IMacro
    {
        public string Id
        {
            get { return "Image"; }
        }

        public IList<MacroRule> Rules
        {
            get
            {
                return new List<MacroRule>()
                           {
                               new MacroRule(
                                   @"(?i)(<\[image:)((?>https?://[^\]\|]*\|https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageWithLinkNoAltLeftAlign},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(>\[image:)((?>https?://[^\]\|]*\|https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageWithLinkNoAltRightAlign},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(\[image:)((?>https?://[^\]\|]*\|https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageWithLinkNoAlt},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(<\[image:)((?>[^\]\|]*\|https?://[^\]\|]*\|https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageWithLinkWithAltLeftAlign},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(>\[image:)((?>[^\]\|]*\|https?://[^\]\|]*\|https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageWithLinkWithAltRightAlign},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(\[image:)((?>[^\]\|]*\|https?://[^\]\|]*\|https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageWithLinkWithAlt},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(<\[image:)((?>https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageLeftAlign},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(>\[image:)((?>https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageRightAlign},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(\[image:)((?>https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageNoAlign},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(<\[image:)((?>[^\]\|]*\|https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageLeftAlignWithAlt},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(>\[image:)((?>[^\]\|]*\|https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageRightAlignWithAlt},
                                           {3, ScopeName.Remove}
                                       }
                                   ),
                               new MacroRule(
                                   @"(?i)(\[image:)((?>[^\]\|]*\|https?://[^\]\|]*))(\])",
                                   new Dictionary<int, string>
                                       {
                                           {1, ScopeName.Remove},
                                           {2, ScopeName.ImageNoAlignWithAlt},
                                           {3, ScopeName.Remove}
                                       }
                                   )
                           };
            }
        }
    }
}