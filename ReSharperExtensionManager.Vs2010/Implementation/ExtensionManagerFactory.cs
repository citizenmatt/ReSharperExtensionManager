using System.Linq;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public static class ExtensionManagerFactory
    {
        public static IExtensionManager Create(IReSharperApi resharperApi)
        {
            var dummySourceRepository = new AggregateRepository(Enumerable.Empty<IPackageRepository>());
            return new ExtensionManager(resharperApi, new PackageManager(dummySourceRepository, Paths.LocalRepositoryRoot));
        }
    }
}