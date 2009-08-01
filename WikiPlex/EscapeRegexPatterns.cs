namespace WikiPlex
{
    public static class EscapeRegexPatterns
    {
        public const string CurlyBraceEscape = @"(?:(?:(?<![<>]){{?)(?>[^}]*)(?:}}?(?![<>])))";
        public const string FullEscape = @"(?s){{.*?}}|{"".*?""}|(?<![<>]){(?!"").*?(?<!"")}(?![<>])|\[.*?\]";
    }
}