using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public static class ExtensionLoaderFactory
    {
        public static IExtensionLoader Create(IReSharperApi resharperApi)
        {
            return new ExtensionLoader(resharperApi, new LocalPackageRepository(Paths.LocalRepositoryRoot));
        }
    }
}