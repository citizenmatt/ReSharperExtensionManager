using System;
using CitizenMatt.ReSharper.ExtensionManager.Implementation;

namespace CitizenMatt.ReSharper.ExtensionManager
{
    public static class Bootstrapper
    {
        public static void Initialise(IVisualStudioApi visualStudioApi, Version resharperVersion)
        {
            Initialise(visualStudioApi, resharperVersion, version => new ReSharperApi60(version), ExtensionManagerFactory.Create);
        }

        public static void Initialise(IVisualStudioApi visualStudioApi, Version resharperVersion, Func<Version, IReSharperApi> resharperApiFactory,
            Func<IReSharperApi, IVisualStudioApi, IExtensionManager> extensionManagerFactory)
        {
            var resharperApi = resharperApiFactory(resharperVersion);
            resharperApi.Initialise(() =>
                                        {
                                            var extensionManager = extensionManagerFactory(resharperApi, visualStudioApi);
                                            extensionManager.InitialiseEnvironment();
                                        });
        }
    }
}