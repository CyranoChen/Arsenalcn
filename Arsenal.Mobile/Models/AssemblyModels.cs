using System;
using System.Reflection;

namespace Arsenal.Mobile.Models
{
    public class AssemblyDto
    {
        public AssemblyDto(Assembly assembly)
        {
            if (assembly != null)
            {
                Title =
                    ((AssemblyTitleAttribute) Attribute.GetCustomAttribute(assembly, typeof (AssemblyTitleAttribute)))?
                        .Title;
                Description =
                    ((AssemblyDescriptionAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof (AssemblyDescriptionAttribute)))?.Description;
                Configuration =
                    ((AssemblyConfigurationAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof (AssemblyConfigurationAttribute)))?.Configuration;
                Company =
                    ((AssemblyCompanyAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof (AssemblyCompanyAttribute)))?.Company;
                Product =
                    ((AssemblyProductAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof (AssemblyProductAttribute)))?.Product;
                Copyright =
                    ((AssemblyCopyrightAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof (AssemblyCopyrightAttribute)))?.Copyright;
                Trademark =
                    ((AssemblyTrademarkAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof (AssemblyTrademarkAttribute)))?.Trademark;
                Culture =
                    ((AssemblyCultureAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof (AssemblyCultureAttribute)))?.Culture;
                Version = assembly.GetName().Version?.ToString();
                FileVersion =
                    ((AssemblyFileVersionAttribute)
                        Attribute.GetCustomAttribute(assembly, typeof (AssemblyFileVersionAttribute)))?.Version;
            }
        }

        #region Members and Properties

        //[assembly: AssemblyTitle("Arsenalcn.Core")]
        //[assembly: AssemblyDescription("沪ICP备12045527号")]
        //[assembly: AssemblyConfiguration("webmaster@arsenalcn.com")]
        //[assembly: AssemblyCompany("Arsenal China Official Supporters Club")]
        //[assembly: AssemblyProduct("Arsenalcn.com")]
        //[assembly: AssemblyCopyright("© 2015")]
        //[assembly: AssemblyTrademark("ArsenalCN")]
        //[assembly: AssemblyCulture("")]
        //[assembly: AssemblyVersion("1.8.*")]
        //[assembly: AssemblyFileVersion("1.8.2")]

        public string Title { get; set; }

        public string Description { get; set; }

        public string Configuration { get; set; }

        public string Company { get; set; }

        public string Product { get; set; }

        public string Copyright { get; set; }

        public string Trademark { get; set; }

        public string Culture { get; set; }

        public string Version { get; set; }

        public string FileVersion { get; set; }

        #endregion
    }
}