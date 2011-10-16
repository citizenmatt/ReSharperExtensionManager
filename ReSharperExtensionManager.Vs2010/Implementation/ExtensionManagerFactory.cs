using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public static class ExtensionManagerFactory
    {
        public static IExtensionManager Create(IReSharperApi resharperApi, IVisualStudioApi visualStudioApi)
        {
            var dummySourceRepository = new LocalPackageRepository(@"C:\rsrepo");
            return new ExtensionManager(resharperApi, visualStudioApi, new PackageManager(dummySourceRepository, Paths.LocalRepositoryRoot));
        }
    }
}