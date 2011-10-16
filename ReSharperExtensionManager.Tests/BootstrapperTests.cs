using System;
using CitizenMatt.ReSharper.ExtensionManager.Implementation;
using CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation;
using Moq;
using Xunit;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests
{
    public class BootstrapperTests
    {
        private readonly Version rsVersion = Version.Parse("6.0.0.0");

        [Fact]
        public void ShouldCreateCorrectVersionOfReSharperApi()
        {
            var rsApiFactoryCalled = false;
            Version capturedVersion = null;
            var vsApi = Mock.Of<IVisualStudioApi>();

            Bootstrapper.Initialise(vsApi, rsVersion, version =>
                                                                 {
                                                                     rsApiFactoryCalled = true;
                                                                     capturedVersion = version;
                                                                     return Mock.Of<IReSharperApi>();
                                                                 }, (rsapi, vsapi) => Mock.Of<IExtensionManager>());

            Assert.True(rsApiFactoryCalled);
            Assert.NotNull(capturedVersion);
            Assert.Equal(rsVersion, capturedVersion);
        }

        [Fact]
        public void ShouldInitialiseResharperApi()
        {
            var rsApi = new FakeReSharperApi(rsVersion);
            var vsApi = Mock.Of<IVisualStudioApi>();
            Bootstrapper.Initialise(vsApi, rsVersion, version => rsApi, (rsapi, vsapi) => Mock.Of<IExtensionManager>());

            Assert.True(rsApi.Initialised);
        }

        [Fact]
        public void ShouldCreateExtensionLoaderAfterResharperApiIsInitialised()
        {
            var extensionLoaderFactoryCalled = false;
            var rsApi = new FakeReSharperApi(rsVersion);
            var vsApi = Mock.Of<IVisualStudioApi>();
            IReSharperApi capturedRsApi = null;
            IVisualStudioApi capturedVsApi = null;

            Bootstrapper.Initialise(vsApi, rsVersion, version => rsApi, (rsapi, vsapi) =>
                                                                                   {
                                                                                       extensionLoaderFactoryCalled = true;
                                                                                       capturedRsApi = rsapi;
                                                                                       capturedVsApi = vsapi;
                                                                                       return Mock.Of<IExtensionManager>();
                                                                                   });

            Assert.True(extensionLoaderFactoryCalled);
            Assert.NotNull(capturedRsApi);
            Assert.Equal(rsApi, capturedRsApi);
            Assert.NotNull(capturedVsApi);
            Assert.Equal(vsApi, capturedVsApi);
        }

        [Fact]
        public void ShouldLoadPlugins()
        {
            var rsApi = new FakeReSharperApi(rsVersion);
            var vsApi = Mock.Of<IVisualStudioApi>();
            var extensionLoader = new Mock<IExtensionManager>();

            Bootstrapper.Initialise(vsApi, rsVersion, version => rsApi, (rsapi, vsapi) => extensionLoader.Object);

            extensionLoader.Verify(loader => loader.InitialiseEnvironment());
        }
    }
}