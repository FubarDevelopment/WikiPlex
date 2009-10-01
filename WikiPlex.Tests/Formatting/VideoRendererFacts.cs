using Xunit;
using Xunit.Extensions;
using WikiPlex.Formatting;

namespace WikiPlex.Tests.Formatting
{
    public class VideoRendererFacts
    {
        public class CanExpand
        {
            [Theory]
            [InlineData(ScopeName.FlashVideo)]
            [InlineData(ScopeName.QuickTimeVideo)]
            [InlineData(ScopeName.RealPlayerVideo)]
            [InlineData(ScopeName.WindowsMediaVideo)]
            [InlineData(ScopeName.YouTubeVideo)]
            [InlineData(ScopeName.InvalidVideo)]
            public void Can_resolve_the_video_scope(string scopeName)
            {
                var renderer = new VideoRenderer();

                bool result = renderer.CanExpand(scopeName);

                Assert.True(result);
            }
        }

        public class Expand
        {
            [Fact]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_url_is_not_specified()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.FlashVideo, "foo", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve video macro, invalid parameter 'url'.</span>", output);
            }

            [Fact]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_url_is_not_a_valid_url()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.FlashVideo, "url=foo", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve video macro, invalid parameter 'url'.</span>", output);
            }

            [Fact]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_align_is_not_valid()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.FlashVideo, "url=http://localhost/video,type=flash,align=a", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve video macro, invalid parameter 'align'.</span>", output);
            }

            [Fact]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_align_is_not_left_center_or_right()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.FlashVideo, "url=http://localhost/video,type=flash,align=justify", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve video macro, invalid parameter 'align'.</span>", output);
            }

            [Fact]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_url_contains_codeplex()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.FlashVideo, "url=http://www.codeplex.com/video,type=flash", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve video macro, invalid parameter 'url'.</span>", output);
            }

            [Theory]
            [InlineData("Left")]
            [InlineData("Center")]
            [InlineData("Right")]
            public void Will_parse_the_content_and_render_the_correct_alignment(string align)
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.FlashVideo, "url=http://localhost/video,type=Flash,align=" + align, x => x, x => x);

                Assert.Equal(@"<div class=""video"" style=""text-align:" + align + @"""><span class=""player""><object type=""application/x-shockwave-flash"" height=""285px"" width=""320px"" classid=""CLSID:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0""><param name=""movie"" value=""http://localhost/video""></param><embed type=""application/x-shockwave-flash"" height=""285px"" width=""320px"" src=""http://localhost/video"" pluginspage=""http://macromedia.com/go/getflashplayer"" autoplay=""false"" autostart=""false"" /></object></span><br /><span class=""external""><a href=""http://localhost/video"" target=""_blank"">Launch in another window</a></span></div>"
                            , output);
            }

            [Fact]
            public void Will_parse_the_content_and_render_the_Flash_video_type()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.FlashVideo, "url=http://localhost/video,type=Flash", x => x, x => x);

                Assert.Equal(@"<div class=""video"" style=""text-align:Center""><span class=""player""><object type=""application/x-shockwave-flash"" height=""285px"" width=""320px"" classid=""CLSID:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0""><param name=""movie"" value=""http://localhost/video""></param><embed type=""application/x-shockwave-flash"" height=""285px"" width=""320px"" src=""http://localhost/video"" pluginspage=""http://macromedia.com/go/getflashplayer"" autoplay=""false"" autostart=""false"" /></object></span><br /><span class=""external""><a href=""http://localhost/video"" target=""_blank"">Launch in another window</a></span></div>"
                            , output);
            }

            [Fact]
            public void Will_parse_the_content_and_render_the_Quicktime_video_type()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.QuickTimeVideo, "url=http://localhost/video,type=Quicktime", x => x, x => x);

                Assert.Equal(@"<div class=""video"" style=""text-align:Center""><span class=""player""><object type=""video/quicktime"" height=""285px"" width=""320px"" classid=""CLSID:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B"" codebase=""http://www.apple.com/qtactivex/qtplugin.cab""><param name=""src"" value=""http://localhost/video""></param><param name=""autoplay"" value=""false""></param><embed type=""video/quicktime"" height=""285px"" width=""320px"" src=""http://localhost/video"" pluginspage=""http://www.apple.com/quicktime/download/"" autoplay=""false"" autostart=""false"" /></object></span><br /><span class=""external""><a href=""http://localhost/video"" target=""_blank"">Launch in another window</a></span></div>"
                            , output);
            }

            [Fact]
            public void Will_parse_the_content_and_render_the_Real_video_type()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.RealPlayerVideo, "url=http://localhost/video,type=Real", x => x, x => x);

                Assert.Equal(@"<div class=""video"" style=""text-align:Center""><span class=""player""><object type=""audio/x-pn-realaudio-plugin"" height=""285px"" width=""320px"" classid=""CLSID:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA"" codebase=""""><param name=""src"" value=""http://localhost/video""></param><param name=""autostart"" value=""false""></param><embed type=""audio/x-pn-realaudio-plugin"" height=""285px"" width=""320px"" src=""http://localhost/video"" pluginspage="""" autoplay=""false"" autostart=""false"" /></object></span><br /><span class=""external""><a href=""http://localhost/video"" target=""_blank"">Launch in another window</a></span></div>"
                            , output);
            }

            [Fact]
            public void Will_parse_the_content_and_render_the_Windows_video_type()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.WindowsMediaVideo, "url=http://localhost/video,type=Windows", x => x, x => x);

                Assert.Equal(@"<div class=""video"" style=""text-align:Center""><span class=""player""><object type=""application/x-mplayer2"" height=""285px"" width=""320px"" classid=""CLSID:22D6F312-B0F6-11D0-94AB-0080C74C7E95"" codebase=""http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701""><param name=""fileName"" value=""http://localhost/video""></param><param name=""autostart"" value=""false""></param><embed type=""application/x-mplayer2"" height=""285px"" width=""320px"" src=""http://localhost/video"" pluginspage=""http://www.microsoft.com/windows/windowsmedia/download/default.asp"" autoplay=""false"" autostart=""false"" /></object></span><br /><span class=""external""><a href=""http://localhost/video"" target=""_blank"">Launch in another window</a></span></div>"
                            , output);
            }

            [Fact]
            public void Will_parse_the_content_and_render_the_YouTube_video_type()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.YouTubeVideo, "url=http://www.youtube.com/watch?v=1234,type=YouTube", x => x, x => x);

                Assert.Equal(@"<div class=""video"" style=""text-align:Center""><span class=""player""><object height=""285px"" width=""320px""><param name=""movie"" value=""http://www.youtube.com/v/1234""></param><param name=""wmode"" value=""transparent""></param><embed height=""285px"" width=""320px"" type=""application/x-shockwave-flash"" wmode=""transparent"" src=""http://www.youtube.com/v/1234"" /></object></span><br /><span class=""external""><a href=""http://www.youtube.com/watch?v=1234"" target=""_blank"">Launch in another window</a></span></div>"
                            , output);
            }

            [Fact]
            public void Will_parse_the_content_and_render_an_unresolved_macro_for_the_invalid_video_scope()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.InvalidVideo, "whateveritdoesntmatter", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve video macro, invalid parameter 'type'.</span>", output);
            }

            [Fact]
            public void Will_parse_the_content_and_height_and_width()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.FlashVideo, "url=http://localhost/video,type=Flash,height=50px,width=250px", x => x, x => x);

                Assert.Equal(@"<div class=""video"" style=""text-align:Center""><span class=""player""><object type=""application/x-shockwave-flash"" height=""50px"" width=""250px"" classid=""CLSID:D27CDB6E-AE6D-11cf-96B8-444553540000"" codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0""><param name=""movie"" value=""http://localhost/video""></param><embed type=""application/x-shockwave-flash"" height=""50px"" width=""250px"" src=""http://localhost/video"" pluginspage=""http://macromedia.com/go/getflashplayer"" autoplay=""false"" autostart=""false"" /></object></span><br /><span class=""external""><a href=""http://localhost/video"" target=""_blank"">Launch in another window</a></span></div>"
                            , output);
            }

            [Fact]
            public void Will_parse_the_content_and_render_the_YouTube_with_height_and_width()
            {
                var renderer = new VideoRenderer();

                string output = renderer.Expand(ScopeName.YouTubeVideo, "url=http://www.youtube.com/watch?v=1234,type=YouTube,height=50px,width=250px", x => x, x => x);

                Assert.Equal(@"<div class=""video"" style=""text-align:Center""><span class=""player""><object height=""50px"" width=""250px""><param name=""movie"" value=""http://www.youtube.com/v/1234""></param><param name=""wmode"" value=""transparent""></param><embed height=""50px"" width=""250px"" type=""application/x-shockwave-flash"" wmode=""transparent"" src=""http://www.youtube.com/v/1234"" /></object></span><br /><span class=""external""><a href=""http://www.youtube.com/watch?v=1234"" target=""_blank"">Launch in another window</a></span></div>"
                            , output);
            }
        }
    }
}