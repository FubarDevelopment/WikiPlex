namespace WikiPlex.Common
{
    public class TextPart
    {
        public TextPart(string text, string friendlyText)
        {
            Text = text;
            FriendlyText = friendlyText;
        }

        public string Text { get; private set; }
        public string FriendlyText { get; private set; }
    }
}