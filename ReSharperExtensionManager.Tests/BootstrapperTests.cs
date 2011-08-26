using System;
using CitizenMatt.ReSharper.ExtensionManager.Implementation;
using CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation;
using Moq;
using Xunit;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests
{
    public class BootstrapperTests
    {
        private readonly Version resharperVersion = Version.Parse("6.0.0.0");

        [Fact]
        public void ShouldCreateCorrectVersionOfReSharperApi()
        {
            var resharperApiFactoryCalled = false;
            Version capturedVersion = null;

            Bootstrapper.Initialise(resharperVersion, version =>
                                                          {
                                                              resharperApiFactoryCalled = true;
                                                              capturedVersion = version;
                                                              return Mock.Of<IReSharperApi>();
                                                          }, api => Mock.Of<IExtensionLoader>());

            Assert.True(resharperApiFactoryCalled);
            Assert.NotNull(capturedVersion);
            Assert.Equal(resharperVersion, capturedVersion);
        }

        [Fact]
        public void ShouldInitialiseResharperApi()
        {
            var resharperApi = new FakeReSharperApi(resharperVersion);
            Bootstrapper.Initialise(resharperVersion, version => resharperApi, api => Mock.Of<IExtensionLoader>());

            Assert.True(resharperApi.Initialised);
        }

        [Fact]
        public void ShouldCreateExtensionLoaderAfterResharperApiIsInitialised()
        {
            var extensionLoaderFactoryCalled = false;
            var resharperApi = new FakeReSharperApi(resharperVersion);
            IReSharperApi capturedApi = null;

            Bootstrapper.Initialise(resharperVersion, version => resharperApi, api =>
                                                                                   {
                                                                                       extensionLoaderFactoryCalled = true;
                                                                                       capturedApi = api;
                                                                                       return Mock.Of<IExtensionLoader>();
                                                                                   });

            Assert.True(extensionLoaderFactoryCalled);
            Assert.NotNull(capturedApi);
            Assert.Equal(resharperApi, capturedApi);
        }

        [Fact]
        public void ShouldLoadPlugins()
        {
            var resharperApi = new FakeReSharperApi(resharperVersion);
            var extensionLoader = new Mock<IExtensionLoader>();

            Bootstrapper.Initialise(resharperVersion, version => resharperApi, api => extensionLoader.Object);

            extensionLoader.Verify(loader => loader.LoadPlugins());
        }
    }
}