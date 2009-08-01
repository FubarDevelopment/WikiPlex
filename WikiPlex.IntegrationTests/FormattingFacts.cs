using System;
using System.IO;
using System.Text.RegularExpressions;
using WikiPlex.Formatting;
using WikiPlex.Syndication;
using Xunit;
using Xunit.Extensions;

namespace WikiPlex.IntegrationTests
{
    public class FormattingFacts : IDisposable
    {
        private static readonly Regex WhitespaceRemovalRegex = new Regex(@"\r|\n|\t|\s{3,4}", RegexOptions.Compiled);

        public FormattingFacts()
        {
            Renderers.Register(new SyndicatedFeedRenderer(new LocalXmlReader(), new SyndicationReader()));
        }

        public void Dispose()
        {
            Renderers.Register<SyndicatedFeedRenderer>();
        }

        [Theory]
        [InputData("ContentAlignmentFormatting")]
        [InputData("TextFormatting")]
        [InputData("LinkFormatting")]
        [InputData("ImageFormatting")]
        [InputData("SourceCodeFormatting")]
        [InputData("ListFormatting")]
        [InputData("SyndicatedFeedFormatting")]
        [InputData("SilverlightFormatting")]
        [InputData("VideoFormatting")]
        [InputData("TableFormatting")]
        [InputData("FullTests")]
        [InputData("CatastrophicBacktracking")]
        public void Will_verify_formatting(string inputFile, string expectedFile)
        {
            string expectedText = File.ReadAllText(expectedFile);
            string actualText = new WikiEngine().Render(File.ReadAllText(inputFile));

            // comment out the following lines if you wish to compare
            // the whitespace correctly
            expectedText = WhitespaceRemovalRegex.Replace(expectedText, string.Empty);
            actualText = WhitespaceRemovalRegex.Replace(actualText, string.Empty);

            Assert.Equal(expectedText, actualText);
        }

        [Fact(Skip = "This is only used to test 1-off inputs manually.")]
        //[Fact]
        public void TestIt()
        {
            string path = @"CatastrophicBacktracking\SPaDevToolkitProject";

            string expectedText = File.ReadAllText(@"Data\" + path + ".html");
            string actualText =
                new WikiEngine().Render(File.ReadAllText(@"Data\" + path + ".wiki"));

            // comment out the following lines if you wish to compare
            // the whitespace correctly
            expectedText = WhitespaceRemovalRegex.Replace(expectedText, string.Empty);
            actualText = WhitespaceRemovalRegex.Replace(actualText, string.Empty);

            Assert.Equal(expectedText, actualText);
        }
    }
}