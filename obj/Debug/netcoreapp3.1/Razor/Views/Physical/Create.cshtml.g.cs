#pragma checksum "C:\aWorkspace\DeigCrud\DeigCrud\Views\Physical\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "755b37025a0ac325c7287cf9d16570453cef642c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Physical_Create), @"mvc.1.0.view", @"/Views/Physical/Create.cshtml")]
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
#line 1 "C:\aWorkspace\DeigCrud\DeigCrud\Views\_ViewImports.cshtml"
using DeigCrud.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"755b37025a0ac325c7287cf9d16570453cef642c", @"/Views/Physical/Create.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"79c8810b25e1b3c754082be4e534af7e0975ca83", @"/Views/_ViewImports.cshtml")]
    public class Views_Physical_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DeigCrud.Models.DlViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"container-fluid container\"\">\r\n    <h1>Create A New Meeting</h1>\r\n    <h2 class=\"alert-danger\">");
#nullable restore
#line 5 "C:\aWorkspace\DeigCrud\DeigCrud\Views\Physical\Create.cshtml"
                        Write(ViewBag.Result);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h2>
    <form asp-controller=""Physical"" method=""post"" role=""form"">
        <table>
            <tr>
                <td>Suspended</td>
                <td>Day</td>
                <td>Time</td>
                <td>Town</td>
                <td>Group Name</td>
                <td>Information</td>
                <td>Location</td>
                <td>Type</td>
                <td>Create</td>
            </tr>
            <tr>
                <td>
                    <select asp-for=""SuspendSelect"" asp-items=""");
#nullable restore
#line 21 "C:\aWorkspace\DeigCrud\DeigCrud\Views\Physical\Create.cshtml"
                                                          Write(Model.Suspended);

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                        <option value=\"0\">False</option>\r\n                    </select>\r\n                </td>\r\n                <td>\r\n                    <select asp-for=\"DOWSelect\" class=\"form-control\" asp-items=\"");
#nullable restore
#line 26 "C:\aWorkspace\DeigCrud\DeigCrud\Views\Physical\Create.cshtml"
                                                                            Write(new SelectList(@Model.DOWModel, "Value", "Text"));

#line default
#line hidden
#nullable disable
            WriteLiteral(";\">\r\n                        <option value=\"-\">Select day</option>\r\n                    </select>\r\n                </td>\r\n                <td>\r\n                    <select asp-for=\"TimeSelect\" class=\"form-control\" asp-items=\"");
#nullable restore
#line 31 "C:\aWorkspace\DeigCrud\DeigCrud\Views\Physical\Create.cshtml"
                                                                             Write(new SelectList(@Model.TimeModel, "Value", "Text"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                        <option value=\"0\">Select Time</option>\r\n                    </select>\r\n                </td>\r\n                <td>\r\n                    <select asp-for=\"TownSelect\" class=\"form-control\" asp-items=\"");
#nullable restore
#line 36 "C:\aWorkspace\DeigCrud\DeigCrud\Views\Physical\Create.cshtml"
                                                                             Write(new SelectList(@Model.TownModel, "Value", "Text"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@""">
                        <option value=""-"">Select Town</option>
                    </select>
                </td>
                <td>
                    <input asp-for=""GroupNameSelect"" class=""form-control"" />
                </td>
                <td>
                    <input asp-for=""InformationSelect"" class=""form-control"" />
                </td>
                <td>
                    <input asp-for=""LocationSelect"" class=""form-control"" />
                </td>
                <td>
                    <input asp-for=""TypeSelect"" class=""form-control"" />
                </td>
                <td>
                    <div class=""text-center panel-body"">
                        <button type=""submit"" class=""btn btn-sm btn-primary"">Create</button>
                    </div>
                </td>
            </tr>
        </table>
        <a asp-action=""Index"" class=""btn btn-sm btn-secondary"">Back</a>
        <hr />
        <h2>");
#nullable restore
#line 61 "C:\aWorkspace\DeigCrud\DeigCrud\Views\Physical\Create.cshtml"
       Write(ViewBag.Result);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h2>\r\n    </form>\r\n</div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DeigCrud.Models.DlViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
