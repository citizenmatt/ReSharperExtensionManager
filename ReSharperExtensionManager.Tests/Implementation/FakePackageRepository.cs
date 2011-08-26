using System.Collections.Generic;
using System.Linq;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Tests.Implementation
{
    public class FakePackageRepository : IPackageRepository
    {
        private readonly List<IPackage> packages = new List<IPackage>();

        public FakePackageRepository(string source)
        {
            Source = source;
        }

        public IQueryable<IPackage> GetPackages()
        {
            return packages.AsQueryable();
        }

        public void AddPackage(IPackage package)
        {
            packages.Add(package);
        }

        public void RemovePackage(IPackage package)
        {
            packages.Remove(package);
        }

        public string Source { get; private set; }
    }
}