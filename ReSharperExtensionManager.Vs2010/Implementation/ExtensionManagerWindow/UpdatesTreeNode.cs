using System.Linq;
using Microsoft.VisualStudio.ExtensionsExplorer;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class UpdatesTreeNode : SimpleTreeNode
    {
        private readonly IPackageManager packageManager;

        public UpdatesTreeNode(string name, IVsExtensionsTreeNode parent, IPackageManager packageManager, IPackageItemCommandHandler commandHandler)
            : base(name, parent, packageManager.SourceRepository, commandHandler)
        {
            this.packageManager = packageManager;
        }

        protected override IQueryable<IPackage> GetPackages()
        {
            return packageManager.SourceRepository.GetUpdates(packageManager.LocalRepository.GetPackages()).AsQueryable();
        }
    }
}