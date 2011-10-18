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
        public void Should_create_correct_cersion_of_resharper_api()
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
        public void Should_initialise_resharper_api()
        {
            var rsApi = new FakeReSharperApi(rsVersion);
            var vsApi = Mock.Of<IVisualStudioApi>();
            Bootstrapper.Initialise(vsApi, rsVersion, version => rsApi, (rsapi, vsapi) => Mock.Of<IExtensionManager>());

            Assert.True(rsApi.Initialised);
        }

        [Fact]
        public void Should_create_extension_manager_after_resharper_api_is_initialised()
        {
            var extensionManagerFactoryCalled = false;
            var rsApi = new FakeReSharperApi(rsVersion);
            var vsApi = Mock.Of<IVisualStudioApi>();
            IReSharperApi capturedRsApi = null;
            IVisualStudioApi capturedVsApi = null;

            Bootstrapper.Initialise(vsApi, rsVersion, version => rsApi, (rsapi, vsapi) =>
                                                                                   {
                                                                                       extensionManagerFactoryCalled = true;
                                                                                       capturedRsApi = rsapi;
                                                                                       capturedVsApi = vsapi;
                                                                                       return Mock.Of<IExtensionManager>();
                                                                                   });

            Assert.True(extensionManagerFactoryCalled);
            Assert.NotNull(capturedRsApi);
            Assert.Equal(rsApi, capturedRsApi);
            Assert.NotNull(capturedVsApi);
            Assert.Equal(vsApi, capturedVsApi);
        }

        [Fact]
        public void Should_load_plugins_at_initialisation()
        {
            var rsApi = new FakeReSharperApi(rsVersion);
            var vsApi = Mock.Of<IVisualStudioApi>();
            var extensionManager = new Mock<IExtensionManager>();

            Bootstrapper.Initialise(vsApi, rsVersion, version => rsApi, (rsapi, vsapi) => extensionManager.Object);

            extensionManager.Verify(m => m.InitialiseEnvironment());
        }

        [Fact]
        public void Should_dispose_of_extension_manager_when_resharper_api_terminates()
        {
            var rsApi = new FakeReSharperApi(rsVersion);
            var vsApi = Mock.Of<IVisualStudioApi>();
            var extensionManager = new Mock<IExtensionManager>();

            Bootstrapper.Initialise(vsApi, rsVersion, version => rsApi, (rsapi, vsapi) => extensionManager.Object);
            rsApi.FakeReSharperTermination();

            extensionManager.Verify(m => m.Dispose());
        }
    }
}