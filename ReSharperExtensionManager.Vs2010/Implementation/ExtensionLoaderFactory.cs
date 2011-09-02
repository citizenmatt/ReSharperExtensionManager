using System.Linq;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation
{
    public static class ExtensionLoaderFactory
    {
        public static IExtensionLoader Create(IReSharperApi resharperApi)
        {
            var dummySourceRepository = new AggregateRepository(Enumerable.Empty<IPackageRepository>());
            return new ExtensionLoader(resharperApi, new PackageManager(dummySourceRepository, Paths.LocalRepositoryRoot));
        }
    }
}