// https://smartflow.wordpress.com/2015/09/01/embed-asp-net-mvc-views-in-class-library-dll/

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;

namespace Fanex.NAP.Core.SignIn
{
    public class EmbeddedResourcePathConfiguration
    {
        public EmbeddedResourcePathConfiguration()
        {
            this.RootNameSpace = "Fanex.NAP.Core.SignIn";
            this.ViewFolderName = "Views";
            this.ResourceAssembly = typeof(EmbeddedResourcePathConfiguration).Assembly;
        }

        public string RootNameSpace { get; set; }
        public string ViewFolderName { get; set; }
        public Assembly ResourceAssembly { get; set; }
        public string ViewNameSpace
        {
            get
            {
                return RootNameSpace + "." + ViewFolderName;
            }
        }

        internal static EmbeddedResourcePathConfiguration Instance = new EmbeddedResourcePathConfiguration();
    }

    public class EmbeddedResourceNameProvider
    {
        public string GetResourceName(string virtualPath)
        {
            var resourcename = virtualPath
                .Substring(virtualPath.IndexOf(EmbeddedResourcePathConfiguration.Instance.ViewNameSpace));

            return resourcename;
        }
    }

    public class EmbeddedVirtualFile : VirtualFile
    {
        public EmbeddedVirtualFile(string virtualPath)
            : base(virtualPath)
        {
        }

        EmbeddedResourceNameProvider embeddedResourceNameProvider = new EmbeddedResourceNameProvider();
        public EmbeddedResourceNameProvider EmbeddedResourceNameProvider
        {
            get { return embeddedResourceNameProvider; }
            set { embeddedResourceNameProvider = value; }
        }

        public override Stream Open()
        {
            string resourcePath = EmbeddedResourceNameProvider.GetResourceName(VirtualPath);
            return EmbeddedResourcePathConfiguration.Instance.ResourceAssembly.GetManifestResourceStream(resourcePath);
        }
    }
    public class EmbeddedViewPathProvider : VirtualPathProvider
    {
        EmbeddedResourceNameProvider embeddedResourceNameProvider = new EmbeddedResourceNameProvider();
        public EmbeddedResourceNameProvider EmbeddedResourceNameProvider
        {
            get { return embeddedResourceNameProvider; }
            set { embeddedResourceNameProvider = value; }
        }

        public EmbeddedViewPathProvider()
        {
        }

        private bool IsVirtualFile(string virtualPath)
        {
            return virtualPath.Contains(EmbeddedResourcePathConfiguration.Instance.ViewNameSpace);
        }

        private bool ResourceFileExists(string virtualPath)
        {
            var resourcename = EmbeddedResourceNameProvider.GetResourceName(virtualPath);
            var result = resourcename != null && EmbeddedResourcePathConfiguration.Instance.ResourceAssembly.GetManifestResourceNames().Contains(resourcename);
            return result;
        }

        public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsVirtualFile(virtualPath))
            {
                return new System.Web.Caching.CacheDependency(EmbeddedResourcePathConfiguration.Instance.ResourceAssembly.Location);
            }
            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override bool FileExists(string virtualPath)
        {
            if (IsVirtualFile(virtualPath))
            {
                bool exists = ResourceFileExists(virtualPath);
                return exists;
            }
            else
            {
                return base.FileExists(virtualPath);
            }
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsVirtualFile(virtualPath))
            {
                return new EmbeddedVirtualFile(virtualPath);
            }
            else
            {
                return base.GetFile(virtualPath);
            }
        }
    }
}
