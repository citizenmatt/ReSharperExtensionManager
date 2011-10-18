using System.IO;
using System.Linq;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public class ExtensionManager : IExtensionManager
    {
        private readonly IReSharperApi resharperApi;
        private readonly IVisualStudioApi visualStudioApi;
        private readonly IPackageManager packageManager;

        public ExtensionManager(IReSharperApi resharperApi, IVisualStudioApi visualStudioApi, IPackageManager packageManager)
        {
            this.resharperApi = resharperApi;
            this.visualStudioApi = visualStudioApi;
            this.packageManager = packageManager;
        }

        public void InitialiseEnvironment()
        {
            RemoveDelayedDeletedExtensions();
            LoadPlugins();
            AddMenuItems();
        }

        public void Dispose()
        {
            RemoveMenuItems();
        }

        private void RemoveDelayedDeletedExtensions()
        {
            var installedPackageDirectories = from package in packageManager.LocalRepository.GetPackages()
                                              select packageManager.PathResolver.GetPackageDirectory(package);
            var allFileSystemDirectories = packageManager.FileSystem.GetDirectories(string.Empty);

            // How odd. Calling Except as an extension method gives a compiler error that XElement needs
            // to included. Calling it as a static method works fine.
            var delayedDeletedPackages = Enumerable.Except(allFileSystemDirectories, installedPackageDirectories);
            foreach (var directory in delayedDeletedPackages.ToList())
            {
                packageManager.FileSystem.DeleteDirectory(directory, true);
            }
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

        private void AddMenuItems()
        {
            resharperApi.AddManagerMenuItem("Manage E&xtensions...", ShowExtensionManagerWindow);
        }

        private void RemoveMenuItems()
        {
            resharperApi.RemoveManagerMenuItem();
        }

        private void ShowExtensionManagerWindow()
        {
            var window = new ExtensionManagerWindow.ExtensionManagerWindow(packageManager);
            window.ShowModal();
            if (window.ShouldRestart)
                visualStudioApi.Restart();
        }

        private string PluginFolder
        {
            get { return string.Format(@"rs{0}{1}\plugins", resharperApi.Version.Major, resharperApi.Version.Minor); }
        }
    }
}