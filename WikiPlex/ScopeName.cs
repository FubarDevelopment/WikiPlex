﻿namespace WikiPlex
{
    /// <summary>
    /// The collection of scope names used during parsing and rendering.
    /// </summary>
    public class ScopeName
    {
#pragma warning disable 1591
        public const string AlignEnd = "Align End Tag";
        public const string Anchor = "Anchor Tag";
        public const string BoldBegin = "Bolded Begin Tag";
        public const string BoldEnd = "Bolded End Tag";
        public const string Channel9Video = "Channel 9 Video";
        public const string ColorCodeAshx = "Color Coding Block - ASHX";
        public const string ColorCodeAspxCs = "Color Coding Block - ASPX and C#";
        public const string ColorCodeAspxVb = "Color Coding Block - ASPX and VB.NET";
        public const string ColorCodeCSharp = "Color Coding Block - C#";
        public const string ColorCodeCpp = "Color Coding Block - C++";
        public const string ColorCodeCss = "Color Coding Block - CSS";
        public const string ColorCodeHtml = "Color Coding Block - HTML";
        public const string ColorCodeJava = "Color Coding Block - Java";
        public const string ColorCodeJavaScript = "Color Coding Block - JavaScript";
        public const string ColorCodeTypeScript = "Color Coding Block - TypeScript";
        public const string ColorCodeFSharp = "Color Coding Block - FSharp";
        public const string ColorCodePhp = "Color Coding Block - PHP";
        public const string ColorCodePowerShell = "Color Coding Block - PowerShell";
        public const string ColorCodeSql = "Color Coding Block - SQL";
        public const string ColorCodeVbDotNet = "Color Coding Block - Visual Basic .NET";
        public const string ColorCodeXml = "Color Coding Block - XML";
        public const string ColorCodeMarkdown = "Color Coding Block - Markdown";
        public const string ColorCodeHaskell = "Color Coding Block - Haskell";
        public const string ColorCodeKoka = "Color Coding Block - Koka";
        public const string EscapedMarkup = "Escaped Markup Block";
        public const string FlashVideo = "Flash Video";
        public const string HeadingFiveBegin = "Heading Five Begin Tag";
        public const string HeadingFiveEnd = "Heading Five End Tag";
        public const string HeadingFourBegin = "Heading Four Begin Tag";
        public const string HeadingFourEnd = "Heading Four End Tag";
        public const string HeadingOneBegin = "Heading One Begin Tag";
        public const string HeadingOneEnd = "Heading One End Tag";
        public const string HeadingSixBegin = "Heading Six Begin Tag";
        public const string HeadingSixEnd = "Heading Six End Tag";
        public const string HeadingThreeBegin = "Heading Three Begin Tag";
        public const string HeadingThreeEnd = "Heading Three End Tag";
        public const string HeadingTwoBegin = "Heading Two Begin Tag";
        public const string HeadingTwoEnd = "Heading Two End Tag";
        public const string HorizontalRule = "Horizontal Rule Tag";
        public const string ImageLeftAlign = "Image Tag - Left Alignment";
        public const string ImageLeftAlignWithAlt = "Image Tag With Alt - Left Alignment";
        public const string ImageNoAlign = "Image Tag - No Alignment";
        public const string ImageNoAlignWithAlt = "Image Tag With Alt - No Alignment";
        public const string ImageRightAlign = "Image Tag - Right Alignment";
        public const string ImageRightAlignWithAlt = "Image Tag With Alt - Right Alignment";
        public const string ImageWithLinkNoAlt = "Image Tag With Link And No Alt";
        public const string ImageWithLinkNoAltLeftAlign = "Image Tag With Link And No Alt - Left Alignment";
        public const string ImageWithLinkNoAltRightAlign = "Image Tag With Link And No Alt - Right Alignment";
        public const string ImageWithLinkWithAlt = "Image Tag With Link And With Alt";
        public const string ImageWithLinkWithAltLeftAlign = "Image Tag With Link And With Alt - Left Alignment";
        public const string ImageWithLinkWithAltRightAlign = "Image Tag With Link And With Alt - Right Alignment";
        public const string ImageDataLeftAlign = "Image Data Tag - Left Alignment";
        public const string ImageDataLeftAlignWithAlt = "Image Data Tag With Alt - Left Alignment";
        public const string ImageDataNoAlign = "Image Data Tag - No Alignment";
        public const string ImageDataNoAlignWithAlt = "Image Data Tag With Alt - No Alignment";
        public const string ImageDataRightAlign = "Image Data Tag - Right Alignment";
        public const string ImageDataRightAlignWithAlt = "Image Data Tag With Alt - Right Alignment";
        public const string ImageDataWithLinkNoAlt = "Image Data Tag With Link And No Alt";
        public const string ImageDataWithLinkNoAltLeftAlign = "Image Data Tag With Link And No Alt - Left Alignment";
        public const string ImageDataWithLinkNoAltRightAlign = "Image Data Tag With Link And No Alt - Right Alignment";
        public const string ImageDataWithLinkWithAlt = "Image Data Tag With Link And With Alt";
        public const string ImageDataWithLinkWithAltLeftAlign = "Image Data Tag With Link And With Alt - Left Alignment";
        public const string ImageDataWithLinkWithAltRightAlign = "Image Data Tag With Link And With Alt - Right Alignment";
        public const string IndentationBegin = "Indentation Begin Tag";
        public const string IndentationEnd = "Indentation End Tag";
        public const string InvalidVideo = "Invalid Video";
        public const string ItalicsBegin = "Italicized Begin Tag";
        public const string ItalicsEnd = "Italicized End Tag";
        public const string LeftAlign = "Left Align Tag";
        public const string LinkAsMailto = "Hyperlink As Mailto, No Text";
        public const string LinkNoText = "Hyperlink With No Text";
        public const string LinkToAnchor = "Link To Anchor";
        public const string LinkWithText = "Hyperlink With Text Tag";
        public const string ListItemBegin = "List Item Begin Tag";
        public const string ListItemEnd = "List Item End Tag";
        public const string MultiLineCode = "Multi Line Code Block";
        public const string OrderedListBeginTag = "Ordered List Begin Tag";
        public const string OrderedListEndTag = "Ordered List End Tag";
        public const string QuickTimeVideo = "QuickTime Video";
        public const string RealPlayerVideo = "Real Player Video";
        public const string Remove = "Text to Remove";
        public const string RightAlign = "Right Alignment Tag";
        public const string Silverlight = "Silverlight Block";
        public const string SingleLineCode = "Single Line Code Block";
        public const string StrikethroughBegin = "Strikethrough Begin Tag";
        public const string StrikethroughEnd = "Strikethrough End Tag";
        public const string SubscriptBegin = "Subscript Begin Tag";
        public const string SubscriptEnd = "Subscript End Tag";
        public const string SuperscriptBegin = "Superscript Begin Tag";
        public const string SuperscriptEnd = "Superscript End Tag";
        public const string SyndicatedFeed = "Syndicated Feed";
        public const string TableBegin = "Table Begin Tag";
        public const string TableCell = "Table Cell End and Begin Tag";
        public const string TableCellHeader = "Table Cell Header Tag";
        public const string TableEnd = "Table End Tag";
        public const string TableRowBegin = "Table Row Begin Tag";
        public const string TableRowEnd = "Table Row End Tag";
        public const string TableRowHeaderBegin = "Table Row Header Begin Tag";
        public const string TableRowHeaderEnd = "Table Row Header End Tag";
        public const string UnderlineBegin = "Underline Begin Tag";
        public const string UnderlineEnd = "Underline End Tag";
        public const string UnorderedListBeginTag = "Unordered List Begin Tag";
        public const string UnorderedListEndTag = "Unordered List End Tag";
        public const string VimeoVideo = "Vimeo Video";
        public const string WindowsMediaVideo = "Windows Media Video";
        public const string YouTubeVideo = "YouTube Video";
#pragma warning restore 1591
    }
}