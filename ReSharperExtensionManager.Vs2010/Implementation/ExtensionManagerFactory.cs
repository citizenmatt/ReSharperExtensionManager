using System.Linq;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public static class ExtensionManagerFactory
    {
        public static IExtensionManager Create(IReSharperApi resharperApi)
        {
            var dummySourceRepository = new LocalPackageRepository(@"C:\rsrepo");
            return new ExtensionManager(resharperApi, new PackageManager(dummySourceRepository, Paths.LocalRepositoryRoot));
        }
    }
}