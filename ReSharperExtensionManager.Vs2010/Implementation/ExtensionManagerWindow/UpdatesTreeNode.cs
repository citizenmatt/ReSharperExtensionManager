using System.Linq;
using Microsoft.VisualStudio.ExtensionsExplorer;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class UpdatesTreeNode : SimpleTreeNode
    {
        private readonly IPackageManager packageManager;
        private readonly IPackageItemCommandHandler commandHandler;

        public UpdatesTreeNode(string name, IVsExtensionsTreeNode parent, IPackageManager packageManager, IPackageItemCommandHandler commandHandler)
            : base(name, parent, packageManager.SourceRepository, commandHandler)
        {
            this.packageManager = packageManager;
            this.commandHandler = commandHandler;
        }

        protected override IQueryable<IPackage> GetPackages()
        {
            return packageManager.SourceRepository.GetUpdates(packageManager.LocalRepository.GetPackages()).AsQueryable();
        }

        protected override PackageItem CreatePackageItem(IPackage package)
        {
            return new PackageItem(package, commandHandler, true);
        }
    }
}