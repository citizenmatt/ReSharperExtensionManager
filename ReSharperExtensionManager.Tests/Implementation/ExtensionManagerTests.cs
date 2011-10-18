using System;
using System.Collections.Generic;
using System.IO;
using NuGet;
using Xunit;
using System.Linq;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class ExtensionManagerTests
    {
        private const string RepoPath = @"C:\temp\repo";

        private readonly ExtensionManager.Implementation.ExtensionManager manager;
        private readonly FakeReSharperApi resharperApi;
        private readonly IPackageManager packageManager;
        private readonly FakePackageRepository localRepository;
        private readonly FakeFileSystem fileSystem;

        public ExtensionManagerTests()
        {
            resharperApi = new FakeReSharperApi(Version.Parse("6.0.0.0"));

            localRepository = new FakePackageRepository(RepoPath);
            fileSystem = new FakeFileSystem(RepoPath);
            var pathResolver = new DefaultPackagePathResolver(fileSystem);
            packageManager = new PackageManager(new AggregateRepository(Enumerable.Empty<IPackageRepository>()), pathResolver, fileSystem, localRepository);
            manager = new ExtensionManager.Implementation.ExtensionManager(resharperApi, null, packageManager);
        }

        [Fact]
        public void Should_not_add_plugins_if_repository_has_no_packages()
        {
            Assert.Equal(0, localRepository.GetPackages().Count());
            manager.InitialiseEnvironment();
            Assert.Equal(0, resharperApi.Plugins.Count);
        }

        private string GetExtensionPath(string id, string version, string path)
        {
            return Path.Combine(RepoPath, Path.Combine(id + "." + version, path));
        }

        [Fact]
        public void Should_add_plugin_with_assembly_files()
        {
            const string id = "packageName";
            const string version = "1.0";
            var files = new List<string>
                            {
                                @"rs60\plugins\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, version, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, version, files[0]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_add_plugin_with_assembly_files_with_different_case()
        {
            const string id = "packageName";
            const string version = "1.0";
            var files = new List<string>
                            {
                                @"RS60\PLUGINS\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, version, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, version, files[0]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_add_plugin_with_all_assembly_files()
        {
            const string id = "packageName";
            const string version = "1.0";
            var files = new List<string>
                            {
                                @"rs60\plugins\plugin.netmodule",
                                @"rs60\plugins\plugin.resource",
                                @"rs60\plugins\plugin.manifest"
                            };
            localRepository.AddPackage(new FakePackage(id, version, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, version, files[0]), resharperApi.Plugins[0].AssemblyFiles);
            Assert.Contains(GetExtensionPath(id, version, files[1]), resharperApi.Plugins[0].AssemblyFiles);
            Assert.Contains(GetExtensionPath(id, version, files[2]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_only_add_plugin_files_from_correct_resharper_version_folder()
        {
            const string id = "packageName";
            const string version = "1.0";
            var files = new List<string>
                            {
                                @"rs60\plugins\plugin.dll",
                                @"rs61\plugins\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, version, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, version, files[0]), resharperApi.Plugins[0].AssemblyFiles);
            Assert.DoesNotContain(GetExtensionPath(id, version, files[1]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_only_add_plugin_files_from_versioned_plugin_folder()
        {
            const string id = "packageName";
            const string version = "1.0";
            var files = new List<string>
                            {
                                @"rs60\plugins\plugin.dll",
                                @"rs60\other\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, version, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, version, files[0]), resharperApi.Plugins[0].AssemblyFiles);
            Assert.DoesNotContain(GetExtensionPath(id, version, files[1]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_not_add_plugin_if_no_files_in_version_specific_plugin_folder()
        {
            const string id = "packageName";
            var files = new List<string>
                            {
                                @"rs60\other\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, "1.0", files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(0, resharperApi.Plugins.Count);
        }

        [Fact]
        public void Should_add_packages_as_enabled()
        {
            const string id = "packageName";

            localRepository.AddPackage(new FakePackage(id, "1.0", @"rs60\plugins\plugin.dll"));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.True(resharperApi.Plugins[0].Enabled);
        }

        [Fact]
        public void Should_add_plugin_for_each_package_in_repository()
        {
            const string packageId1 = "test1";
            const string packageId2 = "test2";
            const string packageId3 = "test3";
            const string packageId4 = "test4";

            const string pluginFile = @"rs60\plugins\plugin.dll";

            localRepository.AddPackage(new FakePackage(packageId1, "1.0", pluginFile));
            localRepository.AddPackage(new FakePackage(packageId2, "1.0", pluginFile));
            localRepository.AddPackage(new FakePackage(packageId3, "1.0", pluginFile));
            localRepository.AddPackage(new FakePackage(packageId4, "1.0", pluginFile));

            manager.InitialiseEnvironment();

            Assert.Equal(4, resharperApi.Plugins.Count);
            Assert.Equal(packageId1, resharperApi.Plugins[0].Id);
            Assert.Equal(packageId2, resharperApi.Plugins[1].Id);
            Assert.Equal(packageId3, resharperApi.Plugins[2].Id);
            Assert.Equal(packageId4, resharperApi.Plugins[3].Id);
        }

        [Fact]
        public void Should_cleanup_all_unused_non_package_directories_during_initialisation()
        {
            fileSystem.Files.Add(@"in_use_package.1.0\blah\blah\blah");
            fileSystem.Files.Add(@"deleted_package.2.1\blah\blah\blah");

            localRepository.AddPackage(new FakePackage("in_use_pacakge", "1.0", new[] { @"rs60\plugings\in_use_package.dll" })); 
            
            manager.InitialiseEnvironment();
        }

        [Fact]
        public void Should_add_extension_manager_menu_item()
        {
            manager.InitialiseEnvironment();

            const string label = "Manage E&xtensions...";
            Assert.Equal(label, resharperApi.ManagerMenuItemLabel);
            Assert.NotNull(resharperApi.ManagerMenuItemAction);
        }

        [Fact]
        public void Should_remove_extension_manager_menu_item_on_dispose()
        {
            manager.InitialiseEnvironment();
            manager.Dispose();

            Assert.Equal(null, resharperApi.ManagerMenuItemLabel);
            Assert.Null(resharperApi.ManagerMenuItemAction);
        }
    }
}