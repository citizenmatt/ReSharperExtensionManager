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

        public ExtensionManagerTests()
        {
            resharperApi = new FakeReSharperApi(Version.Parse("6.0.0.0"));

            localRepository = new FakePackageRepository(RepoPath);
            var fileSystem = new PhysicalFileSystem(RepoPath);
            var pathResolver = new DefaultPackagePathResolver(fileSystem);
            packageManager = new PackageManager(new AggregateRepository(Enumerable.Empty<IPackageRepository>()), pathResolver, fileSystem, localRepository);
            manager = new ExtensionManager.Implementation.ExtensionManager(resharperApi, packageManager);
        }

        [Fact]
        public void Should_not_add_plugins_if_repository_has_no_packages()
        {
            Assert.Equal(0, localRepository.GetPackages().Count());
            manager.InitialiseEnvironment();
            Assert.Equal(0, resharperApi.Plugins.Count);
        }

        private string GetExtensionPath(string id, string path)
        {
            return Path.Combine(RepoPath, Path.Combine(id + ".0.0", path));
        }

        [Fact]
        public void Should_add_plugin_with_assembly_files()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs60\plugins\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, files[0]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_add_plugin_with_assembly_files_with_different_case()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"RS60\PLUGINS\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, files[0]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_add_plugin_with_all_assembly_files()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs60\plugins\plugin.netmodule",
                                @"rs60\plugins\plugin.resource",
                                @"rs60\plugins\plugin.manifest"
                            };
            localRepository.AddPackage(new FakePackage(id, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, files[0]), resharperApi.Plugins[0].AssemblyFiles);
            Assert.Contains(GetExtensionPath(id, files[1]), resharperApi.Plugins[0].AssemblyFiles);
            Assert.Contains(GetExtensionPath(id, files[2]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_only_add_plugin_files_from_correct_resharper_version_folder()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs60\plugins\plugin.dll",
                                @"rs61\plugins\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, files[0]), resharperApi.Plugins[0].AssemblyFiles);
            Assert.DoesNotContain(GetExtensionPath(id, files[1]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_only_add_plugin_files_from_versioned_plugin_folder()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs60\plugins\plugin.dll",
                                @"rs60\other\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(GetExtensionPath(id, files[0]), resharperApi.Plugins[0].AssemblyFiles);
            Assert.DoesNotContain(GetExtensionPath(id, files[1]), resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_not_add_plugin_if_no_files_in_version_specific_plugin_folder()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs60\other\plugin.dll"
                            };
            localRepository.AddPackage(new FakePackage(id, files.ToArray()));

            manager.InitialiseEnvironment();

            Assert.Equal(0, resharperApi.Plugins.Count);
        }

        [Fact]
        public void Should_add_packages_as_enabled()
        {
            const string id = "test1";

            localRepository.AddPackage(new FakePackage(id, @"rs60\plugins\plugin.dll"));

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

            localRepository.AddPackage(new FakePackage(packageId1, pluginFile));
            localRepository.AddPackage(new FakePackage(packageId2, pluginFile));
            localRepository.AddPackage(new FakePackage(packageId3, pluginFile));
            localRepository.AddPackage(new FakePackage(packageId4, pluginFile));

            manager.InitialiseEnvironment();

            Assert.Equal(4, resharperApi.Plugins.Count);
            Assert.Equal(packageId1, resharperApi.Plugins[0].Id);
            Assert.Equal(packageId2, resharperApi.Plugins[1].Id);
            Assert.Equal(packageId3, resharperApi.Plugins[2].Id);
            Assert.Equal(packageId4, resharperApi.Plugins[3].Id);
        }

        [Fact]
        public void Should_add_extenion_manager_menu_item()
        {
            manager.InitialiseEnvironment();

            const string label = "Manage Extensions...";
            Assert.True(resharperApi.Actions.ContainsKey(label));
            Assert.NotNull(resharperApi.Actions[label]);
        }
    }
}