#pragma checksum "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "60f3f21299d678026e61d25421e8e15431ce95f2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_CertBody_Index), @"mvc.1.0.view", @"/Views/CertBody/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\azhar.yusof\Documents\BLMS\Views\_ViewImports.cshtml"
using BLMS;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\azhar.yusof\Documents\BLMS\Views\_ViewImports.cshtml"
using BLMS.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"60f3f21299d678026e61d25421e8e15431ce95f2", @"/Views/CertBody/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1935fafd0aa5610a1ac330bdda92ea4db0ded552", @"/Views/_ViewImports.cshtml")]
    public class Views_CertBody_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<BLMS.Models.Admin.CertBody>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-outline-dark tempting-azure-gradient btn-block text-white text-sm font-weight-bold"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("font-size: 12px;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/mdb/css/addons/datatables.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/mdb/css/addons/datatables-select.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
  
    ViewData["Title"] = "Certificate Body";
    Layout = "~/Views/Shared/_AdminLayout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""container-fluid mt-5 mt-md-0 mb-0"">
    <!-- Grid row -->
    <div class=""row"" style=""margin-top: -100px;"">
        <!-- Grid column -->
        <div class=""col-md-12 px-lg-5"">
            <!-- Card -->
            <div class=""card pb-5 mx-md-3"">
                <div class=""card-body"">
                    <div class=""container text-center my-5"">
                        <h2 class=""title font-weight-bold my-3 wow fadeIn"" data-wow-delay=""0.2s"">
                            <strong>CERTIFICATE BODY</strong>
                        </h2>

                        <p class=""grey-text w-responsive mx-auto mb-3 wow fadeIn"" data-wow-delay=""0.2s"">
                            In this gridview, user can track Certificate Body recorded in BLMS system.
                        </p>

                        <div class=""row wow fadeIn"" data-wow-delay=""0.2s"" style=""font-size: 12px;"">
                            <div class=""col-12"">
                                <div class=""card"">
             ");
            WriteLiteral("                       <div class=\"card-body\">\r\n                                        <div id=\"alert\" class=\"form-group text-left\" style=\"font-size: 14px;\">\r\n                                            ");
#nullable restore
#line 30 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
                                       Write(Html.Raw(@ViewBag.Alert));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                        </div>

                                        <table id=""CertBody"" class=""table table-hover table-wrapper table-striped table-responsive-lg text-left"" cellspacing=""0"" width=""100%"">
                                            <thead>
                                                <tr>
                                                    <th class=""text-sm font-weight-bold col-1"" style=""font-size: 12px;"">No.</th>
                                                    <th class=""text-sm font-weight-bold"" style=""font-size: 12px;"">
                                                        ");
#nullable restore
#line 38 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
                                                   Write(Html.DisplayNameFor(model => model.CertBodyName));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                                    </th>
                                                    <th class=""text-sm font-weight-bold text-center col-2"" style=""font-size: 12px;"">Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
");
#nullable restore
#line 44 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
                                                   int i = 1; 

#line default
#line hidden
#nullable disable
#nullable restore
#line 45 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
                                                 foreach (var item in Model)
                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                    <tr");
            BeginWriteAttribute("id", " id=\"", 2710, "\"", 2735, 2);
            WriteAttributeValue("", 2715, "row_", 2715, 4, true);
#nullable restore
#line 47 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
WriteAttributeValue("", 2719, item.CertBodyID, 2719, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                                        <td class=\"text-sm text-center\" style=\"font-size: 12px;\">\r\n                                                            ");
#nullable restore
#line 49 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
                                                       Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                        </td>\r\n                                                        <td scope=\"row\" class=\"text-sm\" style=\"font-size: 12px;\">\r\n                                                            ");
#nullable restore
#line 52 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
                                                       Write(Html.DisplayFor(modelItem => item.CertBodyName));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                                        </td>
                                                        <td class=""text-center"" style=""font-size: 12px;"">
                                                            <a class=""btn btn-outline-dark winter-neva-gradient btn-rounded btn-sm px-2"" title=""Edit""");
            BeginWriteAttribute("href", " href=\"", 3525, "\"", 3589, 1);
#nullable restore
#line 55 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
WriteAttributeValue("", 3532, Url.Action("Edit", "CertBody", new {id=item.CertBodyID}), 3532, 57, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" style=""font-size: 12px;"">
                                                                <i class=""fas fa-pencil-alt mt-0""></i>
                                                            </a>
                                                            <a class=""btn btn-outline-dark young-passion-gradient btn-rounded btn-sm px-2"" title=""Delete"" href=""#""");
            BeginWriteAttribute("onclick", " onclick=\"", 3950, "\"", 3991, 3);
            WriteAttributeValue("", 3960, "ConfirmDelete(", 3960, 14, true);
#nullable restore
#line 58 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
WriteAttributeValue("", 3974, item.CertBodyID, 3974, 16, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 3990, ")", 3990, 1, true);
            EndWriteAttribute();
            WriteLiteral(@" style=""font-size: 12px;"">
                                                                <i class=""fas fa-trash-alt mt-0""></i>
                                                            </a>
                                                        </td>
                                                    </tr>
");
#nullable restore
#line 63 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
                                                    i++;
                                                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                            </tbody>
                                        </table>

                                        <!--Delete bootstrap confirmation box-->
                                        <div class=""modal fade top"" id=""delete-conformation"" aria-labelledby=""delete-conformation"" aria-hidden=""true"" tabindex=""-1"" role=""dialog"">
                                            <div class=""modal-dialog modal-frame modal-top modal-notify modal-danger"">
                                                <div class=""modal-content"">
                                                    <div class=""modal-header flex-column"">
                                                        <div class=""icon-box"">
                                                            <i class=""material-icons"">&#xE5CD;</i>
                                                        </div>
                                                        <h4 class=""modal-title w-100 font-weight-bolder text-center text-wh");
            WriteLiteral(@"ite"">DELETE CERTIFICATE BODY?</h4>
                                                        <br />
                                                        <p class=""mb-1 align-self-sm-center text-white"" style=""color: red;""><i class=""fas fa-exclamation-circle""></i> The saved data will be permanently deleted from BLMS database.</p>
                                                        <button type=""button"" class=""close"" data-dismiss=""modal"" aria-hidden=""true"">&times;</button>
                                                    </div>
                                                    <div class=""modal-footer justify-content-center"">
                                                        <button type=""button"" class=""btn btn-outline-dark winter-neva-gradient waves-effect"" data-dismiss=""modal"">Cancel</button>
                                                        <a href=""#"" class=""btn btn-outline-dark young-passion-gradient waves-effect"" onclick=""DeleteCertBody()"">Delete</a>
                          ");
            WriteLiteral(@"                          </div>
                                                </div>
                                            </div>
                                        </div>

                                        <input type=""hidden"" id=""hidCertBodyId"" />

                                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "60f3f21299d678026e61d25421e8e15431ce95f215175", async() => {
                WriteLiteral("Create Certificate Body");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

");
            DefineSection("CSS", async() => {
                WriteLiteral("\r\n    <link rel=\"stylesheet\" href=\"https://use.fontawesome.com/releases/v5.11.2/css/all.css\">\r\n\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "60f3f21299d678026e61d25421e8e15431ce95f216972", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "60f3f21299d678026e61d25421e8e15431ce95f218151", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n    <style>\r\n        .select-wrapper input.select-dropdown {\r\n            font-size: 12px;\r\n        }\r\n\r\n        .dropdown-content li > span {\r\n            font-size: 12px;\r\n        }\r\n    </style>\r\n");
            }
            );
            WriteLiteral("\r\n");
            DefineSection("Scripts", async() => {
                WriteLiteral(@"
    <script>
        new WOW().init();

        var ConfirmDelete = function (CertBodyID) {

            $(""#hidCertBodyId"").val(CertBodyID);
            $(""#delete-conformation"").modal('show');
        }

        var DeleteCertBody = function () {
            var CertBodyID = $(""#hidCertBodyId"").val();

            $.ajax({

                type: ""POST"",
                url: '");
#nullable restore
#line 136 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
                 Write(Url.Action("Delete", "CertBody"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"',
                dataType: ""json"",
                data: { Id: CertBodyID },
                success: function (result) {
                    $(""#delete-conformation"").modal(""hide"");
                    $(""#row_"" + CertBodyID).remove();
                    window.location.href =  '");
#nullable restore
#line 142 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
                                        Write(Url.Action("Index", "CertBody"));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"'
                }
            })
        }

        $(document).ready(function () {
            $('#CertBody').DataTable();
            $('#CertBody_wrapper').find('label').each(function () {
                $(this).parent().append($(this).children());
            });
            $('#CertBody_wrapper .dataTables_filter').find('input').each(function () {
                $('input').attr(""placeholder"", ""Search"");
                $('input').attr(""style"", 'font-size: 12px');
                $('input').removeClass('form-control-sm');
            });
            $('#CertBody_wrapper .dataTables_length').addClass('d-flex flex-row');
            $('#CertBody_wrapper .dataTables_filter').addClass('md-form');
            $('#CertBody_wrapper select').removeClass(
                'custom-select custom-select-sm form-control form-control-sm');
            $('#CertBody_wrapper select').addClass('mdb-select');
            $('#CertBody_wrapper .mdb-select').materialSelect();
            $('#CertBody_wr");
                WriteLiteral(@"apper .dataTables_filter').find('label').remove();
        });

        //auto hide viewbag.alert
        $(document).ready(function () {
            setTimeout(function () {
                $(""#alert"").fadeOut();
            }, 3000);
        });
    </script>

");
#nullable restore
#line 174 "C:\Users\azhar.yusof\Documents\BLMS\Views\CertBody\Index.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<BLMS.Models.Admin.CertBody>> Html { get; private set; }
    }
}
#pragma warning restore 1591
