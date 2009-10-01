﻿using Xunit;
using Xunit.Extensions;
using WikiPlex.Formatting;

namespace WikiPlex.Tests.Formatting
{
    public class SilverlightRendererFacts
    {
        public class CanExpand
        {
            [Fact]
            public void Should_indicate_that_renderer_can_resolve_silverlight_scope()
            {
                var renderer = new SilverlightRenderer();

                bool result = renderer.CanExpand(ScopeName.Silverlight);

                Assert.True(result);
            }
        }

        public class Expand
        {
            [Fact]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_url_is_not_specified()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "foo", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve silverlight macro, invalid parameter 'url'.</span>", output);
            }

            [Fact]
            public void Will_parse_the_content_and_return_an_unresolved_macro_is_url_is_not_a_valid_url()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=foo", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve silverlight macro, invalid parameter 'url'.</span>", output);
            }

            [Fact]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_height_is_not_a_number()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,height=a", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve silverlight macro, invalid parameter 'height'.</span>", output);
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_height_is_invalid(int height)
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,height=" + height, x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve silverlight macro, invalid parameter 'height'.</span>", output);
            }

            [Fact]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_width_is_not_a_number()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,width=a", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve silverlight macro, invalid parameter 'width'.</span>", output);
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Will_parse_the_content_and_return_an_unresolved_macro_if_width_is_invalid(int width)
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,width=" + width, x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve silverlight macro, invalid parameter 'width'.</span>", output);
            }

            [Fact]
            public void Will_not_allow_an_url_from_codeplex()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://codeplex.com/foo", x => x, x => x);

                Assert.Equal("<span class=\"unresolved\">Cannot resolve silverlight macro, invalid parameter 'url'.</span>", output);
            }

            [Fact]
            public void Will_render_the_silverlight_object_with_the_default_width_and_height()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight", x => x, x => x);

                Assert.Equal(@"<object data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" style=""height:200px;width:200px;""><param name=""source"" value=""http://localhost/silverlight""></param><param name=""windowless"" value=""true""></param><param name=""minRuntimeVersion"" value=""3.0.40624.0""></param><param name=""autoUpgrade"" value=""true""></param><p>You need to install Microsoft Silverlight to view this content. <a href=""http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0"" style=""text-decoration:none;"">Get Silverlight!<br /><img src=""http://go.microsoft.com/fwlink/?LinkID=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none;"" /></a></p></object><iframe style=""visibility:hidden;height:0;width:0;border-width:0;""></iframe>", output);
            }

            [Fact]
            public void Will_render_the_silverlight_object_with_the_custom_width_and_height()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,height=30,width=40", x => x, x => x);

                Assert.Equal(@"<object data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" style=""height:30px;width:40px;""><param name=""source"" value=""http://localhost/silverlight""></param><param name=""windowless"" value=""true""></param><param name=""minRuntimeVersion"" value=""3.0.40624.0""></param><param name=""autoUpgrade"" value=""true""></param><p>You need to install Microsoft Silverlight to view this content. <a href=""http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0"" style=""text-decoration:none;"">Get Silverlight!<br /><img src=""http://go.microsoft.com/fwlink/?LinkID=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none;"" /></a></p></object><iframe style=""visibility:hidden;height:0;width:0;border-width:0;""></iframe>", output);
            }

            [Fact]
            public void Will_render_the_silverlight_object_with_percentage_width()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,width=100%", x => x, x => x);

                Assert.Equal(@"<object data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" style=""height:200px;width:100%;""><param name=""source"" value=""http://localhost/silverlight""></param><param name=""windowless"" value=""true""></param><param name=""minRuntimeVersion"" value=""3.0.40624.0""></param><param name=""autoUpgrade"" value=""true""></param><p>You need to install Microsoft Silverlight to view this content. <a href=""http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0"" style=""text-decoration:none;"">Get Silverlight!<br /><img src=""http://go.microsoft.com/fwlink/?LinkID=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none;"" /></a></p></object><iframe style=""visibility:hidden;height:0;width:0;border-width:0;""></iframe>", output);
            }

            [Fact]
            public void Will_render_the_silverlight_object_with_percentage_height()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,height=100%", x => x, x => x);

                Assert.Equal(@"<object data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" style=""height:100%;width:200px;""><param name=""source"" value=""http://localhost/silverlight""></param><param name=""windowless"" value=""true""></param><param name=""minRuntimeVersion"" value=""3.0.40624.0""></param><param name=""autoUpgrade"" value=""true""></param><p>You need to install Microsoft Silverlight to view this content. <a href=""http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0"" style=""text-decoration:none;"">Get Silverlight!<br /><img src=""http://go.microsoft.com/fwlink/?LinkID=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none;"" /></a></p></object><iframe style=""visibility:hidden;height:0;width:0;border-width:0;""></iframe>", output);
            }

            [Fact]
            public void Will_render_silverlight_two()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,version=2", x => x, x => x);

                Assert.Equal(@"<object data=""data:application/x-silverlight,"" type=""application/x-silverlight"" style=""height:200px;width:200px;""><param name=""source"" value=""http://localhost/silverlight""></param><param name=""windowless"" value=""true""></param><p>You need to install Microsoft Silverlight to view this content. <a href=""http://go.microsoft.com/fwlink/?LinkID=124807"" style=""text-decoration:none;"">Get Silverlight!<br /><img src=""http://go.microsoft.com/fwlink/?LinkID=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none;"" /></a></p></object><iframe style=""visibility:hidden;height:0;width:0;border-width:0;""></iframe>", output);
            }

            [Fact]
            public void Will_render_the_silverlight_three()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,height=30,width=40,version=3", x => x, x => x);

                Assert.Equal(@"<object data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" style=""height:30px;width:40px;""><param name=""source"" value=""http://localhost/silverlight""></param><param name=""windowless"" value=""true""></param><param name=""minRuntimeVersion"" value=""3.0.40624.0""></param><param name=""autoUpgrade"" value=""true""></param><p>You need to install Microsoft Silverlight to view this content. <a href=""http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0"" style=""text-decoration:none;"">Get Silverlight!<br /><img src=""http://go.microsoft.com/fwlink/?LinkID=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none;"" /></a></p></object><iframe style=""visibility:hidden;height:0;width:0;border-width:0;""></iframe>", output);
            }

            [Fact]
            public void Will_render_the_silverlight_three_object_as_fallback_if_low_version_not_supported()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,version=1", x => x, x => x);

                Assert.Equal(@"<object data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" style=""height:200px;width:200px;""><param name=""source"" value=""http://localhost/silverlight""></param><param name=""windowless"" value=""true""></param><param name=""minRuntimeVersion"" value=""3.0.40624.0""></param><param name=""autoUpgrade"" value=""true""></param><p>You need to install Microsoft Silverlight to view this content. <a href=""http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0"" style=""text-decoration:none;"">Get Silverlight!<br /><img src=""http://go.microsoft.com/fwlink/?LinkID=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none;"" /></a></p></object><iframe style=""visibility:hidden;height:0;width:0;border-width:0;""></iframe>", output);
            }

            [Fact]
            public void Will_render_the_silverlight_three_object_as_fallback_if_high_version_not_supported()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,version=4", x => x, x => x);

                Assert.Equal(@"<object data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" style=""height:200px;width:200px;""><param name=""source"" value=""http://localhost/silverlight""></param><param name=""windowless"" value=""true""></param><param name=""minRuntimeVersion"" value=""3.0.40624.0""></param><param name=""autoUpgrade"" value=""true""></param><p>You need to install Microsoft Silverlight to view this content. <a href=""http://go.microsoft.com/fwlink/?LinkID=149156&v=3.0.40624.0"" style=""text-decoration:none;"">Get Silverlight!<br /><img src=""http://go.microsoft.com/fwlink/?LinkID=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none;"" /></a></p></object><iframe style=""visibility:hidden;height:0;width:0;border-width:0;""></iframe>", output);
            }

            [Fact]
            public void Will_render_the_silverlight_object_with_initParams_included()
            {
                var renderer = new SilverlightRenderer();

                string output = renderer.Expand(ScopeName.Silverlight, "url=http://localhost/silverlight,version=2,param1=a,height=250,width=250,param2=b", x => x, x => x);

                Assert.Equal(@"<object data=""data:application/x-silverlight,"" type=""application/x-silverlight"" style=""height:250px;width:250px;""><param name=""source"" value=""http://localhost/silverlight""></param><param name=""windowless"" value=""true""></param><param name=""initParams"" value=""param1=a,param2=b""></param><p>You need to install Microsoft Silverlight to view this content. <a href=""http://go.microsoft.com/fwlink/?LinkID=124807"" style=""text-decoration:none;"">Get Silverlight!<br /><img src=""http://go.microsoft.com/fwlink/?LinkID=108181"" alt=""Get Microsoft Silverlight"" style=""border-style:none;"" /></a></p></object><iframe style=""visibility:hidden;height:0;width:0;border-width:0;""></iframe>", output);
            }
        }
    }
}