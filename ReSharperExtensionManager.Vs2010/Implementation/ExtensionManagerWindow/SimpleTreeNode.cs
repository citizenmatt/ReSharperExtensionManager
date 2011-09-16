using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.ExtensionsExplorer;
using NuGet;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class SimpleTreeNode : IVsExtensionsTreeNode
    {
        private readonly IPackageManager packageManager;
        private readonly string commandName;
        private IList<IVsExtension> extensions;

        public SimpleTreeNode(string name, IVsExtensionsTreeNode parent, IPackageManager packageManager, string commandName)
        {
            Name = name;
            Parent = parent;
            this.packageManager = packageManager;
            this.commandName = commandName;

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
                    var packages = from package in packageManager.LocalRepository.GetPackages()
                                   select new PackageItem(package, commandName);
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