#pragma checksum "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e51e9ad331b693787fd3420f4e9f799d51a7dc6f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_LicenseApproval_Index), @"mvc.1.0.view", @"/Views/LicenseApproval/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e51e9ad331b693787fd3420f4e9f799d51a7dc6f", @"/Views/LicenseApproval/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1935fafd0aa5610a1ac330bdda92ea4db0ded552", @"/Views/_ViewImports.cshtml")]
    public class Views_LicenseApproval_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<BLMS.Models.License.LicenseApproval>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/mdb/css/addons/datatables.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/mdb/css/addons/datatables-select.min.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
  
    ViewData["Title"] = "License Approval";
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
                            <strong>LICENSE APPROVAL</strong>
                        </h2>

                        <div class=""row wow fadeIn"" data-wow-delay=""0.2s"" style=""font-size: 12px;"">
                            <div class=""col-12"">
                                <div class=""card"">
                                    <div class=""card-body"">
                                        <table id=""LicenseApproval"" class=""table table-hover table-wrapper table-striped table-responsive-lg text-left"" cellspacing=""0"" width=""100%"">
           ");
            WriteLiteral(@"                                 <thead>
                                                <tr>
                                                    <th class=""text-sm font-weight-bold col-1"" style=""font-size: 12px;"">No.</th>
                                                    <th class=""text-sm font-weight-bold"" style=""font-size: 12px;"">
                                                        ");
#nullable restore
#line 30 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                   Write(Html.DisplayNameFor(model => model.UnitName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                    </th>\r\n                                                    <th class=\"text-sm font-weight-bold\" style=\"font-size: 12px;\">\r\n                                                        ");
#nullable restore
#line 33 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                   Write(Html.DisplayNameFor(model => model.LicenseName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                    </th>\r\n                                                    <th class=\"text-sm font-weight-bold\" style=\"font-size: 12px;\">\r\n                                                        ");
#nullable restore
#line 36 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                   Write(Html.DisplayNameFor(model => model.CategoryName));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                                    </th>
                                                    <th class=""text-sm font-weight-bold"" style=""font-size: 12px;"">PIC</th>
                                                </tr>
                                            </thead>
                                            <tbody>
");
#nullable restore
#line 42 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                   int i = 1; 

#line default
#line hidden
#nullable disable
#nullable restore
#line 43 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                 foreach (var item in Model)
                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                    <tr");
            BeginWriteAttribute("id", " id=\"", 2789, "\"", 2813, 2);
            WriteAttributeValue("", 2794, "row_", 2794, 4, true);
#nullable restore
#line 45 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
WriteAttributeValue("", 2798, item.LicenseID, 2798, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                                                        <td class=\"text-sm text-center\" style=\"font-size: 12px;\">\r\n                                                            ");
#nullable restore
#line 47 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                       Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                        </td>\r\n                                                        <td class=\"text-sm col-2\" style=\"font-size: 12px;\">\r\n                                                            ");
#nullable restore
#line 50 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                       Write(Html.DisplayFor(modelItem => item.UnitName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                        </td>\r\n");
            WriteLiteral("                                                        <td class=\"text-sm col-4\" style=\"font-size: 12px;\">\r\n                                                            <a");
            BeginWriteAttribute("href", " href=\"", 3582, "\"", 3652, 1);
#nullable restore
#line 54 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
WriteAttributeValue("", 3589, Url.Action("View", "LicenseApproval", new {id=item.LicenseID}), 3589, 63, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"font-weight-bold\">\r\n                                                                ");
#nullable restore
#line 55 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                           Write(Html.DisplayFor(modelItem => item.LicenseName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n                                                                <br />\r\n\r\n");
#nullable restore
#line 59 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                                 if (item.isRequested == true)
                                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                    <span class=\"badge bg-primary text-sm\">Requested</span>\r\n");
#nullable restore
#line 62 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                                }
                                                                else if (item.isApproved == true)
                                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                    <span class=\"badge bg-info text-sm\">Approved</span>\r\n");
#nullable restore
#line 66 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                                }
                                                                else if (item.isRejected == true)
                                                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                    <span class=\"badge bg-danger text-sm\">Rejected</span>\r\n");
#nullable restore
#line 70 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                            </a>\r\n                                                        </td>\r\n");
            WriteLiteral("                                                        <td class=\"text-sm\" style=\"font-size: 12px;\">\r\n                                                            ");
#nullable restore
#line 75 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                       Write(Html.DisplayFor(modelItem => item.CategoryName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                                                        </td>\r\n");
            WriteLiteral("                                                        <td class=\"text-sm col-3\" style=\"font-size: 12px;\">\r\n");
#nullable restore
#line 79 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                             if (!string.IsNullOrEmpty(item.PIC1Name))
                                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                <label class=\"font-weight-bold\">PIC 1:</label>\r\n                                                                <br />\r\n");
#nullable restore
#line 83 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                           Write(Html.DisplayFor(modelItem => item.PIC1Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                <br /><br />\r\n");
#nullable restore
#line 85 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 87 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                             if (!string.IsNullOrEmpty(item.PIC2Name))
                                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                <label class=\"font-weight-bold\">PIC 2:</label>\r\n                                                                <br />\r\n");
#nullable restore
#line 91 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                           Write(Html.DisplayFor(modelItem => item.PIC2Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                <br /><br />\r\n");
#nullable restore
#line 93 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 95 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                             if (!string.IsNullOrEmpty(item.PIC3Name))
                                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                <label class=\"font-weight-bold\">PIC 3:</label>\r\n                                                                <br />\r\n");
#nullable restore
#line 99 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                           Write(Html.DisplayFor(modelItem => item.PIC3Name));

#line default
#line hidden
#nullable disable
#nullable restore
#line 99 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                                                                            
                                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 102 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                             if (string.IsNullOrEmpty(item.PIC1Name) && string.IsNullOrEmpty(item.PIC2Name) && string.IsNullOrEmpty(item.PIC3Name))
                                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                                <i class=\"text-sm font-italic\" style=\"font-size: 12px;\">N/A</i>\r\n");
#nullable restore
#line 105 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                        </td>\r\n                                                    </tr>\r\n");
#nullable restore
#line 108 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
                                                    i++;
                                                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                                            </tbody>
                                        </table>
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
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "e51e9ad331b693787fd3420f4e9f799d51a7dc6f18787", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "e51e9ad331b693787fd3420f4e9f799d51a7dc6f19966", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
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

        $(document).ready(function () {
            $('#LicenseApproval').DataTable();
            $('#LicenseApproval_wrapper').find('label').each(function () {
                $(this).parent().append($(this).children());
            });
            $('#LicenseApproval_wrapper .dataTables_filter').find('input').each(function () {
                $('input').attr(""placeholder"", ""Search"");
                $('input').attr(""style"", 'font-size: 12px');
                $('input').removeClass('form-control-sm');
            });
            $('#LicenseApproval_wrapper .dataTables_length').addClass('d-flex flex-row');
            $('#LicenseApproval_wrapper .dataTables_filter').addClass('md-form');
            $('#LicenseApproval_wrapper select').removeClass(
                'custom-select custom-select-sm form-control form-control-sm');
            $('#LicenseApproval_wrapper select').addClass('mdb-select');
            $('#LicenseApproval_wrapper .mdb-selec");
                WriteLiteral(@"t').materialSelect();
            $('#LicenseApproval_wrapper .dataTables_filter').find('label').remove();
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
#line 171 "C:\Users\azhar.yusof\Documents\BLMS\Views\LicenseApproval\Index.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<BLMS.Models.License.LicenseApproval>> Html { get; private set; }
    }
}
#pragma warning restore 1591
