using System;
using CitizenMatt.ReSharper.ExtensionManager.Implementation;

namespace CitizenMatt.ReSharper.ExtensionManager
{
    public static class Bootstrapper
    {
        public static void Initialise(Version resharperVersion)
        {
            Initialise(resharperVersion, version => new ReSharperApi(version), resharperApi => new ExtensionLoader(resharperApi));
        }

        public static void Initialise(Version resharperVersion, Func<Version, IReSharperApi> resharperApiFactory,
            Func<IReSharperApi, IExtensionLoader> extensionLoaderFactory)
        {
            var resharperApi = resharperApiFactory(resharperVersion);
            resharperApi.Initialise(() =>
                                        {
                                            var extensionLoader = extensionLoaderFactory(resharperApi);
                                            extensionLoader.LoadPlugins();
                                        });
        }
    }
}