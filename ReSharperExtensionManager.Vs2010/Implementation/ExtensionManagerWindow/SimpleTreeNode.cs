using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.ExtensionsExplorer;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class SimpleTreeNode : IVsExtensionsTreeNode
    {
        private readonly IPackageRepository packageRepository;
        private readonly IPackageItemCommandHandler commandHandler;
        private IList<IVsExtension> extensions;

        public SimpleTreeNode(string name, IVsExtensionsTreeNode parent, IPackageRepository packageRepository, IPackageItemCommandHandler commandHandler)
        {
            Name = name;
            Parent = parent;
            this.packageRepository = packageRepository;
            this.commandHandler = commandHandler;

            Nodes = new ObservableCollection<IVsExtensionsTreeNode>();
        }

        public string Name { get; private set; }
        public IList<IVsExtensionsTreeNode> Nodes { get; private set; }
        public IVsExtensionsTreeNode Parent { get; private set; }

        public IList<IVsExtension> Extensions
        {
            get
            {
                if (extensions == null)
                {
                    extensions = new ObservableCollection<IVsExtension>();
                    var packages = from package in packageRepository.GetPackages()
                                   select new PackageItem(package, commandHandler);
                    extensions.AddRange(packages);
                }
                return extensions;
            }
        }

        public bool IsSearchResultsNode
        {
            get { return false; }
        }

        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
    }
}