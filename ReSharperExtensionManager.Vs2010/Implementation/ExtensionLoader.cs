using System.IO;
using System.Linq;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public class ExtensionLoader : IExtensionLoader
    {
        private readonly IReSharperApi resharperApi;
        private readonly IPackageManager packageManager;

        public ExtensionLoader(IReSharperApi resharperApi, IPackageManager packageManager)
        {
            this.resharperApi = resharperApi;
            this.packageManager = packageManager;
        }

        public void LoadPlugins()
        {
            var packages = packageManager.LocalRepository.GetPackages();
            foreach (var package in packages)
            {
                var packagePath = packageManager.PathResolver.GetInstallPath(package);
                var assemblyFiles = (from packageFile in package.GetFiles(PluginFolder)
                                     select Path.Combine(packagePath, packageFile.Path)).ToList();
                if (assemblyFiles.Count > 0)
                    resharperApi.AddPlugin(package.Id, assemblyFiles, true);
            }
        }

        private string PluginFolder
        {
            get { return string.Format(@"rs{0}{1}\plugins", resharperApi.Version.Major, resharperApi.Version.Minor); }
        }
    }
}