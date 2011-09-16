using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.ExtensionsExplorer;

namespace CitizenMatt.ReSharper.ExtensionManager.Implementation.ExtensionManagerWindow
{
    public class RootTreeNode : IVsExtensionsTreeNode
    {
        public RootTreeNode()
        {
            Nodes = new ObservableCollection<IVsExtensionsTreeNode>();
            Extensions = new ObservableCollection<IVsExtension>();
        }

        public string Name { get; set; }
        public IList<IVsExtensionsTreeNode> Nodes { get; private set; }
        public IVsExtensionsTreeNode Parent { get { return null; }}
        public IList<IVsExtension> Extensions { get; private set; }
        public bool IsSearchResultsNode { get{ return false;}}
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
    }
}