using System.IO;
using System.Linq;
using JetBrains.Util;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public class ExtensionManager : IExtensionManager
    {
        private readonly IReSharperApi resharperApi;
        private readonly IPackageManager packageManager;

        public ExtensionManager(IReSharperApi resharperApi, IPackageManager packageManager)
        {
            this.resharperApi = resharperApi;
            this.packageManager = packageManager;
        }

        public void InitialiseEnvironment()
        {
            LoadPlugins();
            AddMenuItems();
        }

        private void AddMenuItems()
        {
            resharperApi.AddManagerMenuItem("Manage Extensions...", () => MessageBox.ShowExclamation("Managing extensions!"));
        }

        private void LoadPlugins()
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