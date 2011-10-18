#region license
// Copyright 2011 Matt Ellis (@citizenmatt)
//
// This file is part of ReSharper Extension Manager.
//
// ReSharper Extension Manager is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// ReSharper Extension Manager is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with ReSharper Extension Manager.  If not, see <http://www.gnu.org/licenses/>.
#endregion

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