using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eriador.Framework.Services.Settings;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Razor;
using Microsoft.AspNet.Mvc.Razor.OptionDescriptors;
using Microsoft.Framework.OptionsModel;

namespace Eriador.Framework.Theme
{
    public class ThemedRazorViewEngine : RazorViewEngine
    {
        private const string ViewExtension = ".cshtml";
        private readonly IEnumerable<string> _viewLocationFormats;
        private readonly IEnumerable<string> _areaViewLocationFormats;

        public override IEnumerable<string> ViewLocationFormats
        {
            get
            {
                return new[]
                {
                    "/Views/" + SettingsService.GetStatic("system:theme") + "/{1}/{0}" + ViewExtension,
                    "/Views/{1}/{0}" + ViewExtension,
                    "/Views/Shared/{0}" + ViewExtension,
                };
            }
        }

        public override IEnumerable<string> AreaViewLocationFormats
        {
            get
            {
                return new[]
                {
                    "/Areas/{2}/Views/" + SettingsService.GetStatic("system:theme") + "/{1}/{0}" + ViewExtension,
                    "/Areas/{2}/Views/{1}/{0}" + ViewExtension,
                    "/Areas/{2}/Views/Shared/{0}" + ViewExtension,
                    "/Views/Shared/{0}" + ViewExtension,
                };
            }
        }


        public ThemedRazorViewEngine(IRazorPageFactory pageFactory,
                               IRazorViewFactory viewFactory,
                               IViewLocationExpanderProvider viewLocationExpanderProvider,
                               IViewLocationCache viewLocationCache) : 
            base(pageFactory, viewFactory, viewLocationExpanderProvider, viewLocationCache)
        {
            //_viewLocationFormats = new[]
            //{
            //    "/Views/" + Settings.Get("system:theme") + "/{1}/{0}" + ViewExtension,
            //    "/Views/{1}/{0}" + ViewExtension,
            //    "/Views/Shared/{0}" + ViewExtension,
            //};

            //_areaViewLocationFormats = new[]
            //{
            //    "/Areas/{2}/Views/" + Settings.Get("system:theme") + "/{1}/{0}" + ViewExtension,
            //    "/Areas/{2}/Views/{1}/{0}" + ViewExtension,
            //    "/Areas/{2}/Views/Shared/{0}" + ViewExtension,
            //    "/Views/Shared/{0}" + ViewExtension,
            //};
        }
    }
}
