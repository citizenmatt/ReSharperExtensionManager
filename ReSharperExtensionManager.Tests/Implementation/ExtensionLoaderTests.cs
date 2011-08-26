using System;
using System.Collections.Generic;
using CitizenMatt.ReSharper.ExtensionManager.Implementation;
using Xunit;
using System.Linq;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class ExtensionLoaderTests
    {
        private readonly ExtensionLoader loader;
        private readonly FakeReSharperApi resharperApi;
        private readonly FakePackageRepository repository;

        public ExtensionLoaderTests()
        {
            resharperApi = new FakeReSharperApi(Version.Parse("6.0.0.0"));
            repository = new FakePackageRepository(@"C:\temp\repo");
            loader = new ExtensionLoader(resharperApi, repository);
        }

        [Fact]
        public void Should_not_add_plugins_if_repository_has_no_packages()
        {
            Assert.Equal(0, repository.GetPackages().Count());
            loader.LoadPlugins();
            Assert.Equal(0, resharperApi.Plugins.Count);
        }

        //[Fact]
        //public void Should_not_add_plugin_if_no_files_in_version_specific_plugin_folder()
        //{
        //    var files = new List<string>
        //                    {
        //                        @"C:\temp\repo\test1-1.0.0.0\rs6.0\other\readme.txt"
        //                    };
        //    repository.AddPackage(new FakePackage("test1", files.ToArray()));
        //    loader.LoadPlugins();
        //    Assert.Equal(0, resharperApi.Plugins.Count);
        //}

        //[Fact]
        //public void Should_not_add_plugin_if_no_files_in_correct_version_specific_plugin_folder()
        //{
        //    var files = new List<string>
        //                    {
        //                        @"C:\temp\repo\test1-1.0.0.0\rs5.0\other\readme.txt"
        //                    };
        //    repository.AddPackage(new FakePackage("test1", files.ToArray()));
        //    loader.LoadPlugins();
        //    Assert.Equal(0, resharperApi.Plugins.Count);
        //}

        //[Fact]
        //public void Should_not_add_plugin_if_no_dlls_in_version_specific_plugin_folder()
        //{
        //    var files = new List<string>
        //                    {
        //                        @"C:\temp\repo\test1-1.0.0.0\rs6.0\plugins\readme.txt",
        //                        @"C:\temp\repo\test1-1.0.0.0\rs6.0\plugins\boring.txt"
        //                    };
        //    repository.AddPackage(new FakePackage("test1", files.ToArray()));
        //    loader.LoadPlugins();
        //    Assert.Equal(0, resharperApi.Plugins.Count);
        //}

        [Fact]
        public void Should_add_plugin_with_assembly_files()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs6.0\plugins\plugin.dll"
                            };
            repository.AddPackage(new FakePackage(id, files.ToArray()));

            loader.LoadPlugins();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(files[0], resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_add_plugin_with_all_assembly_files()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs6.0\plugins\plugin.netmodule",
                                @"rs6.0\plugins\plugin.resource",
                                @"rs6.0\plugins\plugin.manifest"
                            };
            repository.AddPackage(new FakePackage(id, files.ToArray()));

            loader.LoadPlugins();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(files[0], resharperApi.Plugins[0].AssemblyFiles);
            Assert.Contains(files[1], resharperApi.Plugins[0].AssemblyFiles);
            Assert.Contains(files[2], resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_only_add_plugin_files_from_correct_resharper_version_folder()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs6.0\plugins\plugin.dll",
                                @"rs6.1\plugins\plugin.dll"
                            };
            repository.AddPackage(new FakePackage(id, files.ToArray()));

            loader.LoadPlugins();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(files[0], resharperApi.Plugins[0].AssemblyFiles);
            Assert.DoesNotContain(files[1], resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_only_add_plugin_files_from_versioned_plugin_folder()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs6.0\plugins\plugin.dll",
                                @"rs6.0\other\plugin.dll"
                            };
            repository.AddPackage(new FakePackage(id, files.ToArray()));

            loader.LoadPlugins();

            Assert.Equal(1, resharperApi.Plugins.Count);
            Assert.Equal(id, resharperApi.Plugins[0].Id);
            Assert.Contains(files[0], resharperApi.Plugins[0].AssemblyFiles);
            Assert.DoesNotContain(files[1], resharperApi.Plugins[0].AssemblyFiles);
        }

        [Fact]
        public void Should_not_add_plugin_if_no_files_in_version_specific_plugin_folder()
        {
            const string id = "test1";
            var files = new List<string>
                            {
                                @"rs6.0\other\plugin.dll"
                            };
            repository.AddPackage(new FakePackage(id, files.ToArray()));

            loader.LoadPlugins();

            Assert.Equal(0, resharperApi.Plugins.Count);
        }

        [Fact]
        public void Should_add_packages_as_enabled()
        {
            const string id = "test1";

            repository.AddPackage(new FakePackage(id, @"rs6.0\plugins\plugin.dll"));

            loader.LoadPlugins();

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

            const string pluginFile = @"rs6.0\plugins\plugin.dll";

            repository.AddPackage(new FakePackage(packageId1, pluginFile));
            repository.AddPackage(new FakePackage(packageId2, pluginFile));
            repository.AddPackage(new FakePackage(packageId3, pluginFile));
            repository.AddPackage(new FakePackage(packageId4, pluginFile));

            loader.LoadPlugins();

            Assert.Equal(4, resharperApi.Plugins.Count);
            Assert.Equal(packageId1, resharperApi.Plugins[0].Id);
            Assert.Equal(packageId2, resharperApi.Plugins[1].Id);
            Assert.Equal(packageId3, resharperApi.Plugins[2].Id);
            Assert.Equal(packageId4, resharperApi.Plugins[3].Id);
        }
    }
}